using Final_project.Data;
using Final_project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq; // للتأكد من وجود Linq اللازم لجمع الأخطاء

public class DoctorController : Controller
{
    private readonly AppDbContext _context;

    public DoctorController(AppDbContext context)
    {
        _context = context;
    }

    // عرض ملف الدكتور (Index)
    public async Task<IActionResult> Index(int id)
    {
        var doctor = await _context.Doctors
            // التأكد من جلب بيانات المواعيد والمرضى للعرض في صفحة Index
            .Include(d => d.Appointments)
                .ThenInclude(a => a.Patient)
            .FirstOrDefaultAsync(d => d.Doctor_ID == id);

        if (doctor == null)
            return NotFound();

        return View(doctor);
    }


    // عرض نموذج التعديل (Edit GET)
    [HttpGet]
    public async Task<IActionResult> Edit(int id = 1)
    {
        var doctor = await _context.Doctors.FindAsync(id);
        if (doctor == null)
        {
            TempData["Error"] = "الدكتور غير موجود!";
            return RedirectToAction(nameof(Index), new { id });
        }
        return View(doctor);
    }

    // معالجة نموذج التعديل وإجراء الحفظ (Edit POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        // 💡 الحل: استخدام [Bind] لتحديد الخصائص المسموح بها وتجاهل الخصائص التجميعية
        [Bind("Doctor_ID,Name,Phone,Address,Specialty")] Doctor updated)
    {
        if (!ModelState.IsValid)
        {
            // هذا الجزء يعمل الآن كشبكة أمان لعرض الأخطاء المتبقية
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            TempData["ValidationErrors"] = string.Join(" | ", errors);
            TempData["ErrorMessage"] = "فشل التحقق من صحة البيانات. (الرجاء مراجعة التفاصيل)";

            // العودة إلى نفس الصفحة لعرض الأخطاء تحت الحقول
            return View(updated);

            // ملاحظة: لكي تعمل رسائل الأخطاء تحت الحقول، يجب أن يكون ملف _ValidationScriptsPartial موجوداً في الـ View.
        }

        // جلب الكيان القديم للتعديل
        var doctor = await _context.Doctors.FindAsync(updated.Doctor_ID);
        if (doctor == null)
        {
            TempData["ErrorMessage"] = "حدث خطأ: الدكتور غير موجود في قاعدة البيانات.";
            return RedirectToAction("Index", new { id = updated.Doctor_ID });
        }

        // تحديث الخصائص
        doctor.Name = updated.Name;
        doctor.Phone = updated.Phone;
        doctor.Address = updated.Address;
        doctor.Specialty = updated.Specialty;

        // حفظ التغييرات في قاعدة البيانات
        // _context.Update(doctor); // يمكن الاستغناء عنها لأن الكيان متتبع (Tracked)
        await _context.SaveChangesAsync();

        TempData["Info"] = "Doctoe Updated Successfully";
        return RedirectToAction("Index", new { id = updated.Doctor_ID });
    }
}