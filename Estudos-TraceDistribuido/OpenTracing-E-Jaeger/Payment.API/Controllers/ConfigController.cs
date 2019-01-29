using Microsoft.AspNetCore.Mvc;
using OpenTracing;

namespace Payment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigController
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ConfigController> _logger;
        private readonly ITracer _tracer;

        public ConfigController(IConfiguration configuration, ILogger<ConfigController> logger, ITracer tracer)
        {
            _configuration = configuration;
            _logger = logger;
            _tracer = tracer;
        }

        [HttpGet]
        public IActionResult Test()
        {
            var configs = new List<string>();
            using (_tracer.BuildSpan("SearchConfigs").StartActive(finishSpanOnDispose: true))
            {
                var allChildrens = _configuration.GetChildren();

                foreach (var child in allChildrens)
                {
                    if (child.Value != null)
                    {
                        configs.Add($"{child.Path}:{child.Value}");
                        continue;
                    }

                    configs.AddRange(RunInChildren(child));
                }

                var result = string.Join("\n", configs);

                _logger.LogInformation("Configs: {Result}", result);

                return new OkObjectResult(string.Join("\n", result));
            }
        }

        private static List<string> RunInChildren(IConfigurationSection section)
        {
            var result = new List<string>();

            foreach (var child in section.GetChildren())
            {
                if (child.Value != null)
                {
                    result.Add($"{child.Path}:{child.Value}");
                    continue;
                }

                result.AddRange(RunInChildren(child));
            }

            return result;
        }
    }
}