using System.Collections.Generic;
using System.Threading.Tasks;
using Estudos.AspNetIdentityJwt.Api.Dto;
using Estudos.AspNetIdentityJwt.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Estudos.AspNetIdentityJwt.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;

        public RoleController(RoleManager<Role> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new[] {"value1", "value2"};
        }

        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(RoleDto model)
        {
            var retorno = await _roleManager.CreateAsync(new Role {Name = model.Name});

            return Ok(retorno);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateRole(UpdateUserRoleDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null)
            {
                if (model.Delete)
                    await _userManager.RemoveFromRoleAsync(user, model.Role);
                else
                    await _userManager.AddToRoleAsync(user, model.Role);
            }

            return Ok("FOI");
        }
    }
}