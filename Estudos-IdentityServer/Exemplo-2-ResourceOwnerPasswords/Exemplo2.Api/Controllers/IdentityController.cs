using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Exemplo2.Api.Controllers
{
    public class IdentityController : ControllerBase
    {
        [HttpGet]
        [Route("identity")]
        [Authorize]
        public IActionResult Get()
        {
            return new JsonResult(from c in User.Claims select new {c.Type, c.Value});
        }
    }
}