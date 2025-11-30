using Final_project.Data;
using Final_project.Data.Services_Control;
using Final_project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

public class AppointmentController : Controller
{
    private readonly AppDbContext _context;

    // ملاحظة: حذفنا الـ AppointmentService لأننا بنعمل SaveChanges مباشرة من الـ context
    public AppointmentController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var appointments = _context.Appointments
            .Include(a => a.Patient)
            .Include(a => a.Doctor)
            .Include(a => a.AppointmentServices).ThenInclude(s => s.Service)
            .ToList();
        return View(appointments);
    }

    [HttpGet]
    public IActionResult Book()
    {
        ViewBag.ServicesList = _context.Services.ToList();
        return View(new Appointments());
    }

    [HttpPost]
    public IActionResult Book(string patientEmail, DateTime AppointmentDate, TimeSpan AppointmentTime,
        string? PaymentMethod, string? Notes, List<int> SelectedServices)
    {
        ViewData["patientEmail"] = patientEmail;

        var patient = _context.Patients.FirstOrDefault(p => p.Email == patientEmail);

        if (patient == null)
        {
            ModelState.AddModelError(string.Empty, "Email Does Not Exist, You Should Add The User First");

            ViewBag.ServicesList = _context.Services.ToList();
            var model = new Appointments
            {
                AppointmentDate = AppointmentDate,
                AppointmentTime = AppointmentTime,
                PaymentMethod = PaymentMethod,
                Notes = Notes
            };
            return View(model);
        }

        // إنشاء الموعد
        var appointment = new Appointments
        {
            Patient_ID = patient.Patient_ID,
            Doctor_ID = 1, // دكتور ثابت
            AppointmentDate = AppointmentDate,
            AppointmentTime = AppointmentTime,
            PaymentMethod = PaymentMethod,
            Notes = Notes,
            Status = "Pending"
        };

        _context.Appointments.Add(appointment);
        _context.SaveChanges(); // مهم جدًا

        // إضافة الخدمات
        if (SelectedServices != null)
        {
            foreach (var serviceId in SelectedServices)
            {
                _context.AppointmentServices.Add(new Final_project.Models.AppointmentService
                {
                    App_ID = appointment.App_ID,
                    Service_ID = serviceId
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
        var appointment = _context.Appointments
            .Include(a => a.AppointmentServices)
            .FirstOrDefault(a => a.App_ID == id);

        if (appointment != null)
        {
            _context.AppointmentServices.RemoveRange(appointment.AppointmentServices);
            _context.Appointments.Remove(appointment);
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