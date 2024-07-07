using System.Diagnostics.CodeAnalysis;
using Hangfire;
using Scheduled.Message.Api.Bootstrappers;
using Scheduled.Message.Api.Hangfire.Dashboard.Authorization;
using Scheduled.Message.Api.HealthCheck;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen();

builder.Services.AddHttpContextAccessor();

builder.Services.AddAppHealthChecks(builder.Configuration);

builder.Services.BootstrapperApplication(builder.Configuration);

var app = builder.Build();

app.UseRouting();
app.MapControllers();
app.MapHealthChecks();
app.MapHangfireDashboard(new DashboardOptions
{
    Authorization = new[] { new DashboardNoAuthorizationFilter() },
});

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

using (var scope = app.Services.CreateScope())
{
    MongoDbInitialize.InitializeAsync(scope.ServiceProvider, CancellationToken.None).Wait();
}

app.Run();


namespace Scheduled.Message.Api
{
    [ExcludeFromCodeCoverage]
    public partial class Program;
}