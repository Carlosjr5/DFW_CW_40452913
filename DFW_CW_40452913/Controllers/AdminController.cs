using DFW_CW_40452913.Data;
using DFW_CW_40452913.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DFW_CW_40452913.Controllers
{

    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _dbContext;

        public AdminController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext dbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _dbContext = dbContext;
        }



        public async Task<IActionResult> AdminIndex()
        {
            var users = await _dbContext.Users.ToListAsync();
            var userViewModels = users.Select(u => new getUsersModel
            {
                Id = u.Id,
                UserEmail = u.UserName,
                Roles = _userManager.GetRolesAsync(u).Result.ToList()
            }).ToList();
            return View(userViewModels);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeRole(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, roles);

            await _userManager.AddToRoleAsync(user, roleName);

            return RedirectToAction(nameof(AdminIndex));
        }

        [HttpPost]
        public async Task<IActionResult> BlockUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            await _userManager.SetLockoutEnabledAsync(user, true);
            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);

            //Check json file status
            // return Json(new { success = true });

            // Redirect to the previous page
            return RedirectToAction(nameof(AdminIndex));
        }

     
        [HttpPost]
            public async Task<IActionResult> UnblockUser(string userId)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return NotFound();
                }

                await _userManager.SetLockoutEnabledAsync(user, false);
                await _userManager.ResetAccessFailedCountAsync(user);

                // Store the success message in TempData
                TempData["SuccessMessage"] = "User has been unblocked successfully.";

                // Redirect to the previous page
                return RedirectToAction(nameof(AdminIndex));
            }


    }




}
