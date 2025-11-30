using Final_project.Data; // لو عندك DbContext هنا
using Final_project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Final_project.Controllers
{
    public class BookingController : Controller
    {
        private readonly AppDbContext _context;

        public BookingController(AppDbContext context)
        {
            _context = context;
        }

        // GET: صفحة الهوم (أو أي صفحة فيها الفورم)
        public IActionResult Index()
        {
            // جلب الخدمات من قاعدة البيانات عشان الفورم تعرضهم
            ViewBag.Services = _context.Services.ToList();
            return View(new BookingViewModel()); // تمرير ViewModel فارغ للـ GET
        }

        // POST: إرسال بيانات الحجز
        // باستخدام System.Security.Claims;

        // POST: إرسال بيانات الحجز
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Book(BookingViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Services = _context.Services.ToList();
                return View("Index", model);
            }

            // 1. استخلاص معرف المستخدم المسجل حالياً (User_ID)
            int? currentUserId = null;
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(userIdClaim, out int parsedUserId))
            {
                currentUserId = parsedUserId;
            }

            // ⭐⭐ 2. البحث عن المريض الحالي باستخدام البريد الإلكتروني أو الهاتف ⭐⭐
            var existingPatient = _context.Patients
                .FirstOrDefault(p => p.Email == model.Email || p.Phone == model.Phone);

            Patient patient;

            if (existingPatient != null)
            {
                // 2أ. المريض موجود: نستخدمه.
                patient = existingPatient;

                // تحديث ربط اليوزر إذا كان غير مرتبط و المستخدم الحالي مسجل دخوله
                if (patient.User_ID == null && currentUserId.HasValue)
                {
                    patient.User_ID = currentUserId;
                    // يجب تحديث سجل المريض فقط
                    _context.Patients.Update(patient);
                    _context.SaveChanges();
                }
            }
            else
            {
                // 2ب. المريض غير موجود: ننشئ سجل Patient جديد.
                patient = new Patient
                {
                    fullName = model.FirstName + " " + model.LastName,
                    Email = model.Email,
                    Phone = model.Phone,
                    Gender = model.Gender,
                    Address = "غير محدد",
                    User_ID = currentUserId // نربطه مباشرة باليوزر الحالي
                };
                _context.Patients.Add(patient);
                _context.SaveChanges(); // حفظ المريض الجديد للحصول على Patient_ID
            }

            // 3. إنشاء الحجز (باقي الكود كما هو)
            var appointment = new Appointments
            {
                Patient_ID = patient.Patient_ID, // الآن Patient_ID مضمون سواء كان موجوداً أو جديداً
                Doctor_ID = 1,
                AppointmentDate = model.AppointmentDate.ToDateTime(TimeOnly.MinValue),
                AppointmentTime = model.AppointmentTime.ToTimeSpan(),
                Status = "Pending",
                Notes = model.Notes,
                PaymentMethod = model.PaymentMethod,
                AppointmentServices = new List<AppointmentService>()
            };

            // 4. إضافة الخدمات المختارة
            if (model.SelectedServices != null && model.SelectedServices.Any())
            {
                foreach (var serviceId in model.SelectedServices)
                {
                    if (_context.Services.Any(s => s.Service_ID == serviceId))
                    {
                        appointment.AppointmentServices.Add(new AppointmentService
                        {
                            Service_ID = serviceId
                        });
                    }
                }
            }

            _context.Appointments.Add(appointment);
            _context.SaveChanges();
            TempData["Success"] = "تم حجز الموعد بنجاح!"; // حفظ الموعد والخدمات

            TempData["PatientName"] = patient.fullName;
            TempData["AppointmentDate"] = appointment.AppointmentDate.ToShortDateString();
            TempData["AppointmentTime"] = appointment.AppointmentTime.ToString(@"hh\:mm");
            TempData["Notes"] = appointment.Notes;
            TempData["DoctorName"] = _context.Doctors.FirstOrDefault(d => d.Doctor_ID == appointment.Doctor_ID)?.Name ?? "غير محدد";
            TempData["AppointmentStatus"] = appointment.Status;
            TempData["PaymentMethod"] = appointment.PaymentMethod ?? "غير محدد";
            TempData["ServicesSummary"] = string.Join(", ", appointment.AppointmentServices
                .Select(s => _context.Services.FirstOrDefault(serv => serv.Service_ID == s.Service_ID)?.Name ?? ""));

            return RedirectToAction("Confirmation");
        }

        public IActionResult Confirmation()
        {
            // يمكنك إضافة منطق بسيط للتحقق من وجود البيانات
            if (!TempData.ContainsKey("PatientName"))
            {
                // إعادة التوجيه للصفحة الرئيسية إذا لم تكن هناك بيانات تأكيد حديثة
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}