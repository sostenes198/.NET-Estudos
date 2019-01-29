using System;
using Estudos.Logs.Serilog.Api.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Estudos.Logs.Serilog.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;

        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }
        
        [HttpGet("LogScope")]
        public IActionResult LogScope()
        {
            using (_logger.BeginNamedScope("CasoDeUso", ("Prop1", "55"), ("Prop2", 99)))
            {
                _logger.LogInformation("Scopo de log");
            }
            return Ok();
        }

        [HttpGet("Exception")]
        public IActionResult LogException()
        {
            try
            {
                throw new Exception("Exception", new Exception("Inner Exception"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Deu ruim meu bom");
                throw;
                
            }
        }

        [HttpGet("Information")]
        public IActionResult GetInformation()
        {
            _logger.LogInformation("Informação {numero}", 1);
            return Ok();
        }
        
        [HttpGet("Warning")]
        public IActionResult GetWarning()
        {
            _logger.LogWarning("Warning {numero}", 2);
            return Ok();
        }
        
        [HttpGet("Error")]
        public IActionResult GetError()
        {
            _logger.LogError("Error {numero}", 3);
            return Ok();
        }
        
        [HttpGet("Trace")]
        public IActionResult GetTrace()
        {
            _logger.LogTrace("Trace {numero}", 4);
            return Ok();
        }
        
        [HttpGet("Critical")]
        public IActionResult GetVerbose()
        {
            _logger.LogCritical("Critical {numero}", 5);
            return Ok();
        }
        
        [HttpGet("Debug")]
        public IActionResult GetDebug()
        {
            _logger.LogDebug("Debug {numero}", 6);
            return Ok();
        }
    }
}