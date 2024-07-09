using System.Diagnostics.CodeAnalysis;
using Hangfire;
using NewRelic.LogEnrichers.Serilog;
using Scheduled.Message.Api.Bootstrappers;
using Scheduled.Message.Api.Hangfire.Dashboard.Authorization;
using Scheduled.Message.Api.HealthCheck;
using Scheduled.Message.Api.Serilog;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting host");

    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddControllers();

    builder.Services
        .AddEndpointsApiExplorer()
        .AddSwaggerGen();

    builder.Services.AddHttpContextAccessor();

    builder.Services.AddAppHealthChecks(builder.Configuration);

    builder.Services.BootstrapperApplication(builder.Configuration);
    
    builder.Services.AddSerilog((sp, loggerConfiguration) =>
    {
        var configuration = sp.GetRequiredService<IConfiguration>();

        loggerConfiguration
            .MinimumLevel.Override("Default", Enum.Parse<LogEventLevel>(configuration["LOG_LEVEL_DEFAULT"]!))
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore.HealthChecks.UI", LogEventLevel.Verbose)
            .MinimumLevel.Override("HealthChecks", LogEventLevel.Verbose)
            .MinimumLevel.Override("Hangfire", LogEventLevel.Information);

        loggerConfiguration
            .Enrich.WithNewRelicLogsInContext()
            .Enrich.WithExceptionDetails()
            .Enrich.WithMachineName()
            .Enrich.WithProcessId()
            .Enrich.WithProcessName()
            .Enrich.WithCorrelationId()
            .Enrich.WithCorrelationIdHeader()
            .Enrich.WithThreadId()
            .Enrich.WithThreadName();

        loggerConfiguration.WriteTo.Console(formatter: new NewRelicFormatter());
    });

    var app = builder.Build();

    app.UseSerilogRequestLogging(opts =>
    {
        opts.EnrichDiagnosticContext = SerilogHelper.EnrichFromRequest;
        opts.GetLevel = SerilogHelper.CustomGetLevel;
    });

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
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}


namespace Hangfire.Server.Api
{
    [ExcludeFromCodeCoverage]
    public partial class Program;
}