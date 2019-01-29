using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.Mvc;

namespace Estudos.FeatureFlag.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IFeatureManager _featureManager;

        public TestController(IFeatureManager featureManager)
        {
            _featureManager = featureManager;
        }

        [HttpGet("feature-a")]
        public async Task<IActionResult> FeatureA()
        {
            if (await _featureManager.IsEnabledAsync(MyFeatureFlags.FeatureA))
                return Ok("Feature habilitada A");

            return Ok("Feature Desabilitada A");
        }

        [HttpGet("feature-b")]
        [FeatureGate(MyFeatureFlags.FeatureB)]
        public IActionResult FeatureB()
        {
            return Ok("Feature habilitada B");
        }
    }
}