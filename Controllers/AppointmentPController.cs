using Final_project.Data;
using Final_project.Data.Services_Control;
using Final_project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

public class AppointmentPController : Controller
{
    private readonly AppDbContext _context;

    // ملاحظة: حذفنا الـ AppointmentService لأننا بنعمل SaveChanges مباشرة من الـ context
    public AppointmentPController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var appointments = _context.AppointmentsP
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .Include(a => a.AppointmentPackages).ThenInclude(p => p.Package)
            .ToList();
        return View(appointments);
    }

    [HttpGet]
    public IActionResult Book()
    {
        ViewBag.Packages = _context.Packages.ToList();
        return View(new AppointmentsP());
    }

    [HttpPost]
    public IActionResult Book(string patientEmail, DateTime AppointmentDate, TimeSpan AppointmentTime,
        string? PaymentMethod, string? Notes, List<int> SelectedPackages)
    {
        ViewData["patientEmail"] = patientEmail;

        var patient = _context.Patients.FirstOrDefault(p => p.Email == patientEmail);

        if (patient == null)
        {
            ModelState.AddModelError(string.Empty, "الإيميل غير موجود. يرجى إضافة المريض أولاً.");

            ViewBag.Packages = _context.Packages.ToList();
            var model = new AppointmentsP
            {
                AppointmentDate = AppointmentDate,
                AppointmentTime = AppointmentTime,
                PaymentMethod = PaymentMethod,
                Notes = Notes
            };
            return View(model);
        }

        // إنشاء الموعد
        var appointment = new AppointmentsP
        {
            Patient_ID = patient.Patient_ID,
            Doctor_ID = 1, // دكتور ثابت
            AppointmentDate = AppointmentDate,
            AppointmentTime = AppointmentTime,
            PaymentMethod = PaymentMethod,
            Notes = Notes,
            Status = "Pending"
        };

        _context.AppointmentsP.Add(appointment);
        _context.SaveChanges(); // مهم جدًا

        // إضافة الخدمات
        if (SelectedPackages != null)
        {
            foreach (var packageId in SelectedPackages)
            {
                _context.AppointmentPackages.Add(new Final_project.Models.AppointmentPackages
                {
                    App_ID = appointment.App_ID,
                    Package_ID = packageId
                });
            }
            _context.SaveChanges();
        }
        TempData["Success"] = "The appointment has been successfully booked";
        return RedirectToAction("Index");
    }
    [HttpPost]
    public IActionResult Delete(int id)
    {
        var appointment = _context.AppointmentsP
            .Include(a => a.AppointmentPackages)
            .FirstOrDefault(a => a.App_ID == id);

        if (appointment != null)
        {
            _context.AppointmentPackages.RemoveRange(appointment.AppointmentPackages);

            _context.AppointmentsP.Remove(appointment);
            _context.SaveChanges();
            TempData["Delete"] = "Appointment Deleted Successfully";
        }
        else
        {
            TempData["Error"] = "Appointment Does Not Exist";
        }

        return RedirectToAction("Index");
    }
}
