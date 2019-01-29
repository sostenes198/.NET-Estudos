using Estudos.IdempotentConsumer.DependencyInjection.IdempotentConsumerRepositories;
using Estudos.IdempotentConsumer.Sample.Application;
using Estudos.IdempotentConsumer.Sample.Application.UseCase;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddIdempotentConsumerRepository(new IdempotentConsumerRepositoryOptions(
                                                     new SqlServerOptions(builder.Configuration["ConnectionStrings:SqlServer"]),
                                                     new InMemoryRepositoryOptions(100)));

builder.Services.TryAddScoped<IUseCase, UseCaseWithoutReturn>();
builder.Services.TryAddScoped<IUseCase<Return>, UseCaseWithReturn>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();