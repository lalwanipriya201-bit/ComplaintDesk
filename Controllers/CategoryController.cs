using ComplaintMgtSystem.Data;
using ComplaintMgtSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComplaintMgtSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Display all categories
        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories
                .OrderBy(c => c.Name)
                .ToListAsync();

            return View(categories);
        }

        // Display the Add Category form
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Save a new category
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid)
                return View(category);

            bool exists = await _context.Categories
                .AnyAsync(c => c.Name.ToLower() == category.Name.Trim().ToLower());

            if (exists)
            {
                ModelState.AddModelError(
                    nameof(category.Name),
                    "A category with this name already exists.");

                return View(category);
            }

            category.Name = category.Name.Trim();

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Category added successfully.";

            return RedirectToAction(nameof(Index));
        }

        // Display the Edit Category form
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
                return NotFound();

            return View(category);
        }

        // Save category changes
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (id != category.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(category);

            string name = category.Name.Trim();

            bool exists = await _context.Categories.AnyAsync(
                c => c.Id != id &&
                     c.Name.ToLower() == name.ToLower());

            if (exists)
            {
                ModelState.AddModelError(
                    nameof(category.Name),
                    "A category with this name already exists.");

                return View(category);
            }

            var existingCategory = await _context.Categories.FindAsync(id);

            if (existingCategory == null)
                return NotFound();

            existingCategory.Name = name;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Category updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        // Delete a category only if no complaints use it
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
                return NotFound();

            bool inUse = await _context.Complaints
                .AnyAsync(c => c.CategoryId == id);

            if (inUse)
            {
                TempData["ErrorMessage"] =
                    "This category cannot be deleted because complaints are using it.";

                return RedirectToAction(nameof(Index));
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Category deleted successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}