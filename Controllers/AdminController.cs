using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HomeQuest.Data;
using System.Data;

namespace HomeQuest.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;


        public AdminController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;

        }

        public async Task<IActionResult> AdminDashboard()
        {
            var usersList = await _userManager.GetUsersInRoleAsync("User");
            var agentsList = await _userManager.GetUsersInRoleAsync("Agent");

            return View(new Tuple<List<ApplicationUser>, List<ApplicationUser>>(usersList.ToList(), agentsList.ToList()));
        }

        public async Task<IActionResult> ApproveAgent(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            user.AgentIsApproved = true;
            await _userManager.UpdateAsync(user);
            return RedirectToAction("AdminDashboard");
        }

        public async Task<IActionResult> DisapproveAgent(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            user.AgentIsApproved = false;
            await _userManager.UpdateAsync(user);
            return RedirectToAction("AdminDashboard");
        }


        public async Task<IActionResult> DeactivateUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.LockoutEnabled = true;
            user.LockoutEnd = DateTimeOffset.MaxValue;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest();
            }

            return RedirectToAction("AdminDashboard");
        }

        public async Task<IActionResult> ActivateUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.LockoutEnd = null;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest();
            }

            return RedirectToAction("AdminDashboard");
        }
    }

}

