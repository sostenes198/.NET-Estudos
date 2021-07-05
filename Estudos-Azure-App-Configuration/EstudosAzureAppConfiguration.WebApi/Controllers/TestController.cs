using EstudosAzureAppConfiguration.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace EstudosAzureAppConfiguration.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly Settings settings;

        public TestController(IOptions<Settings> settings)
        {
            this.settings = settings.Value;
        }
        
        [HttpGet("collor")]
        public IActionResult ShowConfigs()
        {
            return Ok(settings);
        }
    }
}