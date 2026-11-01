
using ComplaintMgtSystem.Data;
using ComplaintMgtSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ComplaintMgtSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserManagementController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserManagementController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Display and search registered users
        public async Task<IActionResult> Index(string? search)
        {
            var query = _context.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();

                query = query.Where(u =>
                    u.Name.Contains(search) ||
                    u.Email.Contains(search));
            }

            ViewBag.Search = search;

            var users = await query
                .OrderBy(u => u.Name)
                .Select(u => new
                {
                    u.Id,
                    u.Name,
                    u.Email,
                    u.Role,
                    u.CreatedAt,
                    u.IsActive
                })
                .ToListAsync();

            return View(users);
        }

        // Deactivate a user account
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deactivate(int id)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                TempData["Error"] = "User account not found.";
                return RedirectToAction(nameof(Index));
            }

            string? currentUserId = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            // Prevent an administrator from deactivating themselves.
            if (currentUserId == user.Id.ToString())
            {
                TempData["Error"] =
                    "You cannot deactivate your own account.";

                return RedirectToAction(nameof(Index));
            }

            if (!user.IsActive)
            {
                TempData["Error"] = "This account is already inactive.";
                return RedirectToAction(nameof(Index));
            }

            // Never deactivate the last active administrator.
            if (user.Role == "Admin")
            {
                bool anotherActiveAdminExists =
                    await _context.Users.AnyAsync(u =>
                        u.Role == "Admin" &&
                        u.IsActive &&
                        u.Id != user.Id);

                if (!anotherActiveAdminExists)
                {
                    TempData["Error"] =
                        "You cannot deactivate the last active administrator.";

                    return RedirectToAction(nameof(Index));
                }
            }

            user.IsActive = false;
            await _context.SaveChangesAsync();

            TempData["Success"] =
                "User account deactivated successfully.";

            return RedirectToAction(nameof(Index));
        }

        // Reactivate a user account
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reactivate(int id)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                TempData["Error"] = "User account not found.";
                return RedirectToAction(nameof(Index));
            }

            if (user.IsActive)
            {
                TempData["Error"] = "This account is already active.";
                return RedirectToAction(nameof(Index));
            }

            user.IsActive = true;
            await _context.SaveChangesAsync();

            TempData["Success"] =
                "User account reactivated successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}
