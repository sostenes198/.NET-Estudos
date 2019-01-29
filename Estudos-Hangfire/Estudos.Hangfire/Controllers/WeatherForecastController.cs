using Estudos.Hangfire.Services;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace Estudos.Hangfire.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public void Get([FromHeader] string Test)
    {
        BackgroundJob.Enqueue<ITestService>(t => t.WriteServer1());
    }
}