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
        private readonly IUserClaimsPrincipalFactory<MyUser> _userClaimsPrincipalFactory;
        private readonly SignInManager<MyUser> _signInManager;

        public LoginController(UserManager<MyUser> userManager,
            IUserClaimsPrincipalFactory<MyUser> userClaimsPrincipalFactory,
            SignInManager<MyUser> signInManager)
        {
            _userManager = userManager;
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
            _signInManager = signInManager;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(MyUserViewModel userViewModel)
        {
//            var siginResult = await _signInManager.PasswordSignInAsync(
//                userViewModel.UserName, userViewModel.Password, false, false);

//            if (siginResult.Succeeded)
//                return Ok(siginResult);

            var user = await _userManager.FindByNameAsync(userViewModel.UserName);

            if (user == default)
                return Unauthorized("Usuario ou senha inválido");

            if (!await _userManager.IsEmailConfirmedAsync(user))
                return BadRequest("TEM QUE CONFIRMAR O EMAIL DOIDAO");

            if (await _userManager.IsLockedOutAsync(user))
                return BadRequest("DEU EXTREMAMENTE RUIM VAI TER QUE TROCAR A SENHA VACILAUM");

            if (!await _userManager.CheckPasswordAsync(user, userViewModel.Password))
            {
                await _userManager.AccessFailedAsync(user);
                return Unauthorized("Usuario ou senha inválido");
            }

            await _userManager.ResetAccessFailedCountAsync(user);
            
            var principal = await _userClaimsPrincipalFactory.CreateAsync(user);
            var identity = new ClaimsIdentity("Identity.Application");
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
            identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));

            await HttpContext.SignInAsync("Identity.Application", principal);

            return Ok(principal);
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
                    UserName = userViewModel.UserName,
                    Email = userViewModel.Email
                };

                var result = await _userManager.CreateAsync(model, userViewModel.Password);

                if (result.Succeeded)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(model);
                    var confirmationEmail = Url.Action("ConfirmEmail", "Login", new {token, email = model.Email}, Request.Scheme);

                    System.IO.File.WriteAllText("confirmationEmail.txt", confirmationEmail);
                }

                return Ok(result);
            }

            return BadRequest("Deu ruim");
        }

        [HttpGet]
        [Route("confirmemail")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);

                if (result.Succeeded)
                    return Ok("DEU BOM MEU BOM");
            }

            return BadRequest("DEU RUIM EMU BOM");
        }

        [HttpPost]
        [Route("forgotpassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPassword forgotPassword)
        {
            var user = await _userManager.FindByEmailAsync(forgotPassword.Email);
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetUrl = Url.Action("ResetPassword", "Login", new {token = token, email = forgotPassword.Email}, Request.Scheme);

            System.IO.File.WriteAllText("reset-link.txt", resetUrl);

            return Ok();
        }

        [HttpPost]
        [Route("resetpassword")]
        public async Task<IActionResult> ResetPassword(ResetPassword forgotPassword)
        {
            var user = await _userManager.FindByEmailAsync(forgotPassword.Email);

            var result = await _userManager.ResetPasswordAsync(user, forgotPassword.Token, forgotPassword.Password);

            return Ok();
        }
    }
}