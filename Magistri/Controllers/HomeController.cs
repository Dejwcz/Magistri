using Magistri.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Magistri.Controllers {
    public class HomeController : Controller {
        private readonly ILogger<HomeController> _logger;
        private UserManager<AppUser> _userManager;

        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager) {
            _logger = logger;
            _userManager = userManager;
        }
        [Authorize]
        public async Task<IActionResult> IndexAsync() {
            AppUser user  = await _userManager.GetUserAsync(HttpContext.User);
            string message = $"Hello {user.UserName}";
            return View("Index", message);
        }

        //public IActionResult Privacy() => View();

        public IActionResult Info() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
