using System.Diagnostics;
using Final_project.Data; // 👈 أضف هذا
using Final_project.Models;
using Microsoft.AspNetCore.Mvc;

namespace Final_project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context; // 👈 أضف هذا

        public HomeController(ILogger<HomeController> logger, AppDbContext context) // 👈 عدّل المُنشئ
        {
            _logger = logger;
            _context = context; // 👈 احفظ الـ DbContext
        }

        public IActionResult Index()
        {
            var vm = new DashboardStatsVM();

            // احصائيات من الداتا بيز
            vm.TotalPatients = _context.Patients.Count();
            vm.TotalAppointments = _context.Appointments.Count();
            vm.TotalUsers = _context.Users.Count(); // بدل الإيرادات

            // حساب نسب (اختاري Target زي ما تحبي)
            vm.PatientsPercentage = (int)((vm.TotalPatients / 300.0m) * 100);
            vm.AppointmentsPercentage = (int)((vm.TotalAppointments / 200.0m) * 100);
            vm.UsersPercentage = (int)((vm.TotalUsers / 100.0m) * 100); // نسبة عدد المستخدمين

            // التأكد من الحد الأقصى 100%
            vm.PatientsPercentage = Math.Min(vm.PatientsPercentage, 100);
            vm.AppointmentsPercentage = Math.Min(vm.AppointmentsPercentage, 100);
            vm.UsersPercentage = Math.Min(vm.UsersPercentage, 100);

            return View(vm);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult HomePage()
        {
            ViewBag.Services = _context.Services.ToList();

            return View(); // 👈 هذا يعرض HomePage.cshtml مع ViewBag.Services
        }


    }
}