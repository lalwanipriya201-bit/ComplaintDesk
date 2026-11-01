using ComplaintMgtSystem.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComplaintMgtSystem.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "User")]
        public IActionResult UserDashboard()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminDashboard()
        {
            ViewBag.TotalComplaints =
                await _context.Complaints.CountAsync();

            ViewBag.PendingComplaints =
                await _context.Complaints.CountAsync(c => c.Status == "Pending");

            ViewBag.InProgressComplaints =
                await _context.Complaints.CountAsync(c => c.Status == "In Progress");

            ViewBag.ResolvedComplaints =
                await _context.Complaints.CountAsync(c => c.Status == "Resolved");

            return View();
        }
    }
}