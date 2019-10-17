using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Estudos.AspnetIdentity.Models;
using Estudos.AspnetIdentity.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Estudos.AspnetIdentity.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly UserManager<MyUser> _userManager;

        public LoginController(UserManager<MyUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(MyUserViewModel userViewModel)
        {
            var user = await _userManager.FindByNameAsync(userViewModel.UserName);

            if (user != default && await _userManager.CheckPasswordAsync(user, userViewModel.Password))
            {
                var identity = new ClaimsIdentity("cookies");
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
                identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));

                await HttpContext.SignInAsync("cookies", new ClaimsPrincipal(identity));

                return Ok();
            }

            return Unauthorized("Usuario ou senha inválido");
        }
        
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(MyUserViewModel userViewModel)
        {
            var user = await _userManager.FindByNameAsync(userViewModel.UserName);

            if (user == default)
            {
                var model = new MyUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = userViewModel.UserName
                };

                var result = await _userManager.CreateAsync(model, userViewModel.Password);
            }
            
            return Accepted();
        }
    }
}