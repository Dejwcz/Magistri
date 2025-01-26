using Magistri.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Magistri.Controllers {
    [Authorize(Roles = "Admin")]
    public class RolesController : Controller {
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<AppUser> _userManager;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager) {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Index() => View(_roleManager.Roles);

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> CreateAsync(string name) {
            IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(name));
            if (result.Succeeded) {
                return RedirectToAction("Index");
            }
            else foreach (var error in result.Errors) {
                    ModelState.AddModelError("", error.Description);
                }
            return View(name);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id) {
            IdentityRole foundRole = await _roleManager.FindByIdAsync(id);
            if (foundRole != null) {
                IdentityResult delete = await _roleManager.DeleteAsync(foundRole);
                if (delete.Succeeded) {
                    return RedirectToAction("Index");
                }
                else {
                    foreach (var error in delete.Errors) {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            else {
                ModelState.AddModelError("", "No role found");
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> EditAsync(string id) {
            IdentityRole roleToEdit = await _roleManager.FindByIdAsync(id);
            List<AppUser> members = new();
            List<AppUser> nonmembers = new();
            if (roleToEdit != null) {
                foreach (var user in _userManager.Users) {
                    var list = await _userManager.IsInRoleAsync(user, roleToEdit.Name) ? members : nonmembers;
                    list.Add(user);
                }
                return View(new RoleEdit {
                    Role = roleToEdit,
                    Members = members,
                    NonMembers = nonmembers,
                });
            }
            else {
                ModelState.AddModelError("", "Role not found");
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(RoleModification modification) {
            foreach (string userId in modification.AddIds ?? []) {
                AppUser user = await _userManager.FindByIdAsync(userId);
                if (user != null) {
                    IdentityResult result = await _userManager.AddToRoleAsync(user, modification.RoleName);
                    if (!result.Succeeded) {
                        AddModelsErrors(result);
                    }
                }
            }
            foreach (string userId in modification.DeleteIds ?? Array.Empty<string>()) {
                AppUser user = await _userManager.FindByIdAsync(userId);
                if (user != null) {
                    IdentityResult result = await _userManager.RemoveFromRoleAsync(user, modification.RoleName);
                    if (!result.Succeeded) {
                        AddModelsErrors(result);
                    }
                }
            }
            return RedirectToAction(nameof(Index));
        }

        private void AddModelsErrors(IdentityResult result) {
            foreach (var error in result.Errors) {
                ModelState.AddModelError("", error.Description);
            }
        }
    }
}
