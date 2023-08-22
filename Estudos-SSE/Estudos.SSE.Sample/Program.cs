using Estudos.SSE.Sample;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.DependencyInjection.Extensions;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IHostedService, RedisSubscribe>();

builder.Services.TryAddSingleton<IDistributedCache>(_ =>
{
    return new RedisCache(new RedisCacheOptions
    {
        Configuration = builder.Configuration["Redis"]
    });
});

builder.Services.TryAddSingleton<IConnectionMultiplexer>(sp => ConnectionMultiplexer.Connect(builder.Configuration["Redis"]));


builder.Services.AddSse(opt =>
    {
        opt.CloseConnectionsInMinutesInterval = 1;
        opt.OnClientConnected = (provider, client) =>
        {
            var loggerFactory = provider.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger("OnClientConnected");
            logger.LogInformation("Connected Client {Id}", client.Id);
        };
        opt.OnClientDisconnected = (provider, client) =>
        {
            var loggerFactory = provider.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger("OnClientConnected");
            logger.LogInformation("Desconnected Client {Id}", client.Id);
        };
    })
   .AddEmptyAuthorization()
   .AddNewGuidClientIdProvider()
   .AddDistributedCacheStorage()
   .AddKeepAliveConnections();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(t => t.AllowAnyOrigin());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseSse("/sse-endpoint");

app.Run();