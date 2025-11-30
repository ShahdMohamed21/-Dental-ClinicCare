using Final_project.Data;
using Final_project.Models;
using Final_project.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final_project.Controllers
{
    [Route("Package/[action]")]
    public class PackageController : Controller
    {
        private readonly PackageService _packageService;
        private readonly IWebHostEnvironment _env;
        private readonly AppDbContext _context;

        public PackageController(PackageService packageService, IWebHostEnvironment env)
        {
            _packageService = packageService;
            _env = env;
        }

        // =================== Get All ====================
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _packageService.GetAllPackages();

            if (!result.Success)
            {
                ViewBag.ErrorMessage = result.Message;
                return View(new List<Package>());
            }

            return View(result.Data);
        }

        // =================== Dashboard (Admin View) ====================
        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            var result = await _packageService.GetAllPackages();

            if (!result.Success)
            {
                ViewBag.ErrorMessage = result.Message;
                return View(new List<Package>());
            }

            return View(result.Data);
        }

        // =================== Details ====================
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var result = await _packageService.GetPackageById(id);

            if (!result.Success || result.Data == null)
                return NotFound(result.Message);

            return View(result.Data);
        }

        // =================== Create GET ====================
        [HttpGet]
        public IActionResult Create() => View();

        // =================== Create POST ====================
        [HttpPost]
        public async Task<IActionResult> Create(Package package)
        {
            if (!ModelState.IsValid)
                return View(package);

            var result = await _packageService.AddPackage(package);

            if (!result.Success)
            {
                ViewBag.ErrorMessage = result.Message;
                return View(package);
            }

            TempData["Success"] = "Package added successfully!";
            return RedirectToAction(nameof(Dashboard));
        }

        // =================== Edit GET ====================
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _packageService.GetPackageById(id);

            if (!result.Success || result.Data == null)
                return NotFound(result.Message);

            return View(result.Data);
        }

        // =================== Edit POST ====================
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Package updated)
        {
            if (!ModelState.IsValid)
                return View(updated);

            var result = await _packageService.UpdatePackage(id, updated);

            if (!result.Success)
            {
                ViewBag.ErrorMessage = result.Message;
                return View(updated);
            }

            TempData["Info"] = "Package updated successfully!";
            return RedirectToAction(nameof(Dashboard));
        }

        // =================== Delete ====================
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _packageService.DeletePackage(id);

            if (result.Success)
                TempData["Delete"] = "Package Deleted successfully!";
            else
                TempData["Error"] = "Package Does Not Exist";

            return RedirectToAction(nameof(Dashboard));
        }

        // =================== Search GET ====================
        [HttpGet]
        public IActionResult Search()
        {
            return View();
        }

        // =================== Search POST ====================
        [HttpPost]
        public async Task<IActionResult> Search(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                ViewBag.Message = "Please enter a package name to search.";
                return View(new List<Package>());
            }

            var allPackages = await _packageService.GetAllPackages();

            if (!allPackages.Success)
            {
                ViewBag.Message = allPackages.Message;
                return View(new List<Package>());
            }

            var filtered = allPackages.Data?
                .Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                .ToList()
                ?? new List<Package>();

            ViewBag.SearchTerm = name;

            ViewBag.Message = filtered.Any()
                ? $"Found {filtered.Count} packages matching '{name}'"
                : $"No packages found matching '{name}'";

            return View(filtered);
        }



    }
}