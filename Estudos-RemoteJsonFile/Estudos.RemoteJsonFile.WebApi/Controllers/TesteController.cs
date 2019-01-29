using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Estudos.RemoteJsonFile.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TesteController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public TesteController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public string Get()
        {
            return _configuration["RESULT:DYNAMIC_VALUE"];
        }
    }
}