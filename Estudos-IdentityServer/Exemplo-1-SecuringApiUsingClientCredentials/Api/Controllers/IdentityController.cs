using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class IdentityController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return new JsonResult(from c in User.Claims select new {c.Type, c.Value});
        }
    }
}