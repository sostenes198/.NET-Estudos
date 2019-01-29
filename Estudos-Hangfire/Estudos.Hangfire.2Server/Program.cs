using Estudos.Hangfire._2Server;
using Estudos.Hangfire._2Server.Services;
using Hangfire;
using Hangfire.Common;
using Hangfire.SqlServer;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions
    {
        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
        QueuePollInterval = TimeSpan.Zero,
        UseRecommendedIsolationLevel = true,
        DisableGlobalLocks = true,
        SchemaName = "App_Server2"
    }));

builder.Services.AddHangfireServer(options =>
{
    options.ServerName = "App_Server2";
});

GlobalJobFilters.Filters.Add(new FilterJobsAttribute());

builder.Services.TryAddScoped<ITestService, TestService>();
builder.Services.TryAddScoped<IRepo, Repo>();

builder.Services.AddHttpContextAccessor();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseHangfireDashboard();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

RecurringJob.AddOrUpdate<ITestService>("App_Server2_Test1", t => t.WriteServer2(), Cron.Minutely);
RecurringJob.AddOrUpdate<ITestService>("App_Server2_Test2", t => t.WriteServer2(), Cron.Minutely);

app.Run();