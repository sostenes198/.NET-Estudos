using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Estudos.IdentityServerMvc.Models;
using Microsoft.AspNetCore.Authorization;

namespace Estudos.IdentityServerMvc.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}