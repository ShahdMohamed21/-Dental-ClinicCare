using Final_project.Models;
using Final_project.Services;
using Microsoft.AspNetCore.Mvc;

namespace Final_project.Controllers
{
    public class ServicesController : Controller
    {
        private readonly ServiceService _serviceService;
        private readonly IWebHostEnvironment _env;

        public ServicesController(ServiceService serviceService, IWebHostEnvironment env)
        {
            _serviceService = serviceService;
            _env = env;
        }
        public IActionResult Hollywood()
        {
            return View();
        }

        // زراعة الأسنان
        public IActionResult DentalImplant()
        {
            return View();
        }

        // تقويم الأسنان
        public IActionResult Orthodontics()
        {
            return View();
        }

        // علاج الجذور
        public IActionResult RootTreatment()
        {
            return View();
        }

        // أسنان الأطفال
        public IActionResult ChildDental()
        {
            return View();
        }

        // علاج اللثة
        public IActionResult GumTreatment()
        {
            return View();
        }

        // تبييض الأسنان
        public IActionResult Whitening()
        {
            return View();
        }

        // الحشوات التجميلية
        public IActionResult CosmeticFillings()
        {
            return View();
        }
        // Get All
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _serviceService.GetAllServices();

            if (!result.Success)
            {
                ViewBag.ErrorMessage = result.Message;
                return View(new List<Service>());
            }

            return View(result.Data);
        }

        // Details
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var result = await _serviceService.GetServiceById(id);

            if (!result.Success || result.Data == null)
                return NotFound(result.Message);

            return View(result.Data);
        }

        // Create GET
        [HttpGet]
        public IActionResult Create() => View();

        // ✅ Create POST + Image Upload
        [HttpPost]
        public async Task<IActionResult> Create(Service service)
        {
            if (!ModelState.IsValid)
                return View(service);

            var result = await _serviceService.AddService(service);

            if (!result.Success)
            {
                ViewBag.ErrorMessage = result.Message;
                return View(service);
            }
            

            TempData["Success"] = "Service added successfully!";
            return RedirectToAction(nameof(Index));
        }

        // Edit GET
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _serviceService.GetServiceById(id);
            if (!result.Success || result.Data == null)
                return NotFound(result.Message);

            return View(result.Data);
        }

        // ✅ Edit POST
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Service updated)
        {
            if (!ModelState.IsValid)
                return View(updated);

            var result = await _serviceService.UpdateService(id, updated);

            if (!result.Success)
            {
                ViewBag.ErrorMessage = result.Message;
                return View(updated);
            }
           
            TempData["Info"] = "Service updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        // Delete
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _serviceService.DeleteService(id);

            if (result.Success)
            {
            
                TempData["Delete"] = "Service Deleted Successfully";
            }
            else
                TempData["Error"] = " Service Deletion Failed";

            return RedirectToAction(nameof(Index));
        }

        // Search GET
        [HttpGet]
        public IActionResult Search()
        {
            return View();
        }

        // Search POST
        [HttpPost]
        public async Task<IActionResult> Search(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                ViewBag.Message = "Please enter a service name to search.";
                return View(new List<Service>());
            }

            var allServices = await _serviceService.GetAllServices();

            if (!allServices.Success)
            {
                ViewBag.Message = allServices.Message;
                return View(new List<Service>());
            }

            var filteredServices = allServices.Data?
                .Where(s => s.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                .ToList() ?? new List<Service>();

            ViewBag.SearchTerm = name;
            ViewBag.Message = filteredServices.Any()
                ? $"Found {filteredServices.Count} services matching '{name}'"
                : $"No services found matching '{name}'";

            return View(filteredServices);
        }
    }
}