using Magistri.DTO;
using Magistri.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Magistri.Controllers {
    [Authorize]
    public class AccountController : Controller {
        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager) {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl) {
            LoginDto loginDto = new LoginDto();
            loginDto.ReturnUrl = returnUrl;
            return View(loginDto);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDto loginDto) {
            if (ModelState.IsValid) {
                AppUser appUser = await _userManager.FindByNameAsync(loginDto.UserName);
                if (appUser != null) {
                    SignInResult signInResult = await _signInManager.PasswordSignInAsync(appUser, loginDto.Password, false, false);
                    if (signInResult.Succeeded) {
                        return RedirectToAction("Index", "Home");
                    }
                }
                ModelState.AddModelError("", "Login failed");
            }
            return View(loginDto);
        }

        [HttpGet]
        public IActionResult AccessDenied() => View();

        public async Task<IActionResult> Logout() {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
