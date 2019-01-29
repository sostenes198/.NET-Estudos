using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace EstudosAzureAppConfiguration.WebApi
{
    
    // TODO Listagem de configurações
    public class TesteController
    {
        private readonly IConfiguration _configuration;

        public TesteController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        [HttpGet]
        public IActionResult Test()
        {
            var result = new List<string>();
            var allChildrens = _configuration.GetChildren();

            foreach (var child in allChildrens)
            {
                if (child.Value != null)
                {
                    result.Add($"{child.Path}:{child.Value}");
                    continue;
                }

                result.AddRange(RunInChildren(child));
            }
            
            return new OkObjectResult(string.Join("\n", result));
        }

        private List<string> RunInChildren(IConfigurationSection section)
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