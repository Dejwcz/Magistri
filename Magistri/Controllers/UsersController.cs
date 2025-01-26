using Magistri.DTO;
using Magistri.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Magistri.Controllers {
    [Authorize]
    public class UsersController : Controller {
        private UserManager<AppUser> _userManager;
        private IPasswordHasher<AppUser> _passwordHasher;
        private IPasswordValidator<AppUser> _passwordValidator;

        public UsersController(UserManager<AppUser> userManager, IPasswordHasher<AppUser> passwordHasher, IPasswordValidator<AppUser> passwordValidator) {
            _userManager = userManager;
            _passwordHasher = passwordHasher;
            _passwordValidator = passwordValidator;
        }

        [HttpGet]
        public IActionResult Index() {
            return View(_userManager.Users);
        }

        [HttpGet]
        public IActionResult Create() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(UserDto newUser) {
            if (ModelState.IsValid) {
                AppUser appUser = new AppUser() {
                    UserName = newUser.Name,
                    Email = newUser.Email,
                };
                IdentityResult identityResult = await _userManager.CreateAsync(appUser, newUser.Password);
                if (identityResult.Succeeded) {
                    return RedirectToAction("Index");
                }
                else {
                    foreach (var error in identityResult.Errors) {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(newUser);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id) {
            var appUser = await _userManager.FindByIdAsync(id);
            if (appUser == null) {
                return View(NotFound());
            }
            else {
                return View(appUser);
            }

        }
        [HttpPost]
        public async Task<IActionResult> Edit(string id, string email, string password) {
            AppUser userToEdtit = await _userManager.FindByIdAsync(id);
            if (userToEdtit != null) {
                IdentityResult validPassword = null;
                if (!string.IsNullOrEmpty(email)) {
                    userToEdtit.Email = email;
                }
                else {
                    ModelState.AddModelError("", "Email cannot be empty");
                }
                if (!string.IsNullOrEmpty(password)) {
                    validPassword = await _passwordValidator.ValidateAsync(_userManager, userToEdtit, password);
                    userToEdtit.PasswordHash = _passwordHasher.HashPassword(userToEdtit, password);
                }
                else {
                    ModelState.AddModelError("", "Password cannot be empty");
                }
                if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password)) {
                    if (validPassword != null && validPassword.Succeeded) {
                        IdentityResult result = await _userManager.UpdateAsync(userToEdtit);
                        if (result.Succeeded) {
                            RedirectToAction("Index");
                        }
                        else {
                            foreach (var error in result.Errors) {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }
                        }
                    }
                }
            }
            else {
                ModelState.AddModelError("", "User not found");
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(string id) {
            AppUser appUser = await _userManager.FindByIdAsync(id);
            if (appUser != null) {
                IdentityResult result = await _userManager.DeleteAsync(appUser);
                if (result.Succeeded) {
                    RedirectToAction("Index");
                }
                else {
                    foreach (var error in result.Errors) {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            else {
                ModelState.AddModelError("", "User not found");
            }
            return RedirectToAction("Index");
        }
    }
}
