using Final_project.Data; // لو عندك DbContext هنا
using Final_project.Models;
using Microsoft.AspNetCore.Mvc;

namespace Final_project.Controllers
{
    public class PackageBookingController : Controller
    {
        private readonly AppDbContext _context;

        public PackageBookingController(AppDbContext context)
        {
            _context = context;
        }

        // GET: صفحة الهوم (أو أي صفحة فيها الفورم)
        public IActionResult Index()
        {
            // جلب الخدمات من قاعدة البيانات عشان الفورم تعرضهم
            ViewBag.Packages = _context.Packages.ToList();
            return View(new PackageBookingViewModel()); // تمرير ViewModel فارغ للـ GET
        }

        // POST: إرسال بيانات الحجز
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Book(PackageBookingViewModel model) // غيرت الاسم هنا
        {
            if (!ModelState.IsValid)
            {
                // لو الفورم فيها مشكلة ارجع لنفس الصفحة مع الخدمات
                ViewBag.Packages = _context.Packages.ToList();
                return View("Index", model); // مهم: بترجع الموديل
            }

            // إنشاء Patient جديد أو يمكن عمل Check لو موجود مسبقًا (هنا بننشئ جديد)
            var patient = new Patient
            {
                fullName = model.FirstName + " " + model.LastName,
                Email = model.Email,
                Phone = model.Phone,
                Gender = model.Gender,
                Address = "غير محدد" // ممكن تضيف حقل Address في الفورم لو حابة
            };
            _context.Patients.Add(patient);
            _context.SaveChanges(); // لازم تبقي هنا علشان يتحط ID

            // إنشاء الحجز
            var appointment = new AppointmentsP
            {
                Patient_ID = patient.Patient_ID,
                Doctor_ID = 1, // 👈 إضافة الدكتور الوحيد
                AppointmentDate = model.AppointmentDate.ToDateTime(TimeOnly.MinValue),
                AppointmentTime = model.AppointmentTime.ToTimeSpan(),
                Status = "Pending",
                Notes = model.Notes,
                AppointmentPackages = new List<AppointmentPackages>()
            };
            // Loop على الخدمات المختارة وضيفها لقائمة AppointmentServices
            foreach (var packageId in model.SelectedPackages)
            {
                // تحقق من أن serviceId موجود في قاعدة البيانات (اختياري)
                if (_context.Packages.Any(p => p.Package_ID == packageId))
                {
                    appointment.AppointmentPackages.Add(new AppointmentPackages
                    {
                        Package_ID = packageId
                    });
                }
            }

            _context.AppointmentsP.Add(appointment);
            _context.SaveChanges();

            TempData["PatientName"] = patient.fullName;
            TempData["AppointmentDate"] = appointment.AppointmentDate.ToShortDateString();
            TempData["AppointmentTime"] = appointment.AppointmentTime.ToString(@"hh\:mm");
            TempData["Notes"] = appointment.Notes;

            return RedirectToAction("Index"); // هيروح على نفس Index.cshtml

        }
    }
}