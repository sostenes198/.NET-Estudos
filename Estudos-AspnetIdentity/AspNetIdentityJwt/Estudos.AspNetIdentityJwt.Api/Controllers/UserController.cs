using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Estudos.AspNetIdentityJwt.Api.Dto;
using Estudos.AspNetIdentityJwt.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Estudos.AspNetIdentityJwt.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserController(IConfiguration configuration, UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult User()
        {
            return Ok(new UserDto());
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginDto model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user == null)
                return Unauthorized();

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (result.Succeeded)
            {
                var appUser = await _userManager.Users.FirstOrDefaultAsync(lnq => lnq.NormalizedUserName == user.UserName.ToUpper());

                var token = await GenerateJwtToken(appUser);

                return Ok(token);
            }

            return Unauthorized();
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Post(UserDto model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user == null)
            {
                user = new User
                {
                    UserName = model.UserName,
                    Email = model.Email
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    var appUser = await _userManager.Users.FirstOrDefaultAsync(lnq => lnq.NormalizedUserName == user.UserName.ToUpper());
                    var token = await GenerateJwtToken(appUser);
                    //var confirmationEmail
                }

                return Ok("Cadastrado com sucesso");
            }

            return Unauthorized();
        }

        private async Task<string> GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = cred
                //NotBefore = DateTime.Now,
                //IssuedAt = DateTime.Now,
                //Issuer = "SOSO",
                //Audience = "API RECURSO",
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescription);

            return tokenHandler.WriteToken(token);
        }
    }
}