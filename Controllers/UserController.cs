using Final_project.Data;
using Final_project.Models;
using Final_project.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

public class UserController : Controller
{
    private readonly UserService _userService;
    private readonly AppDbContext _context;

    public UserController(AppDbContext context)
    {
        _context = context;                // ✨ مهم جداً
        _userService = new UserService(context);
    }

    public IActionResult Index()
    {
        var users = _userService.GetAllUsers();
        return View(users);
    }

    // تعديل الإجراء Details في UserController
    public IActionResult Details(int id)
    {
        // 1️⃣ جلب اليوزر
        var user = _context.Users.FirstOrDefault(u => u.User_ID == id);
        if (user == null)
            return NotFound();

        // 2️⃣ جلب جميع سجلات المرضى المرتبطة بهذا اليوزر
        var patientRecords = _context.Patients
            .Where(p => p.User_ID == user.User_ID)
            .ToList();

        // استخلاص قائمة بمعرفات المرضى (Patient_IDs)
        var patientIds = patientRecords.Select(p => p.Patient_ID).ToList();

        // 3️⃣ جلب المواعيد الخاصة بجميع هؤلاء المرضى (باستخدام القائمة patientIds)
        var appointments = new List<Appointments>();

        if (patientIds.Any())
        {
            appointments = _context.Appointments
                .Where(a => patientIds.Contains(a.Patient_ID)) // جلب المواعيد التي Patient_ID الخاص بها موجود في القائمة
                .Include(a => a.Patient) // مهم لربط الموعد باسم المريض (وهو سبب المشكلة)
                .Include(a => a.Doctor)  // يفضل تضمين الطبيب أيضاً
                .Include(a => a.AppointmentServices)
                    .ThenInclude(s => s.Service)
                .OrderByDescending(a => a.AppointmentDate)
                .ToList();
        }

        // 4️⃣ تمرير البيانات للـ ViewModel
        var model = new UserDetailsViewModel
        {
            User = user,
            // يمكنك هنا عرض تفاصيل المريض الرئيسي إذا كان موجوداً
            Patient = patientRecords.FirstOrDefault(),
            Appointments = appointments
        };

        return View(model);
    }



    public IActionResult Delete(int id)
    {
        var user = _userService.GetUserById(id);
        if (user == null) return NotFound();

        return View(user);
    }

    [HttpPost]
    public IActionResult DeleteConfirmed(int id)
    {
        _userService.DeleteUser(id);
        return RedirectToAction("Index");
    }

    // ⭐ صفحة البروفايل
    public IActionResult Profile()
    {
        // 1. استخلاص معرف المستخدم الحالي
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
            return RedirectToAction("Login", "Account"); // أو أي صفحة تسجيل دخول

        // 2. جلب اليوزر وتحميل العلاقات الضرورية (Eager Loading)
        var user = _context.Users
            .Where(u => u.User_ID == userId)
            .Include(u => u.Patients) // تحميل جميع المرضى المرتبطين بهذا اليوزر
                .ThenInclude(p => p.Appointments.OrderByDescending(a => a.AppointmentDate)) // تحميل المواعيد لكل مريض (مرتبة عكسياً)
                    .ThenInclude(a => a.Doctor) // تحميل بيانات الطبيب لكل موعد
            .Include(u => u.Patients)
                .ThenInclude(p => p.Appointments)
                    .ThenInclude(a => a.AppointmentServices) // تحميل خدمات الموعد
                        .ThenInclude(ap => ap.Service) // تحميل اسم الخدمة نفسها
            .FirstOrDefault(); // تنفيذ الاستعلام

        if (user == null)
            return NotFound();

        // 3. تجميع كل المواعيد في قائمة واحدة وعرضها (لا تحتاج إلى SelectMany إذا كان الـ Include يعمل بشكل صحيح)
        var appointments = user.Patients
            // SelectMany يجمع قائمة المواعيد من جميع المرضى في قائمة واحدة
            .SelectMany(p => p.Appointments ?? new List<Appointments>())
            .OrderBy(a => a.AppointmentDate) // إعادة ترتيب المواعيد حسب التاريخ
            .ToList();

        // 4. إنشاء ViewModel وتمريرها
        var model = new UserProfileViewModel
        {
            User = user,
            Appointments = appointments
        };

        return View(model);
    }


    [HttpGet]
    public IActionResult Edit(int id)
    {
        // 1. استخدام _context لجلب بيانات المستخدم مباشرة
        var userToEdit = _context.Users.Find(id);

        if (userToEdit == null)
        {
            return NotFound(); // 404
        }

        // 2. إرسال الكيان إلى View (Edit.cshtml)
        return View(userToEdit);
    }

    // -----------------------------------------------------------------

    // هذا الإجراء يُنفذ عندما يرسل المستخدم النموذج من Edit.cshtml
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(User model)
    {
        // تأكدي أن الـ Model يحتوي على الـ User_ID الصحيح
        if (model.User_ID == 0)
        {
            return BadRequest("معرف المستخدم غير موجود.");
        }

        // 1. التحقق من صحة المدخلات 
        if (ModelState.IsValid)
        {
            try
            {
                // 2. إعلام Context بتحديث الكيان
                _context.Update(model);

                // 3. حفظ التغييرات في قاعدة البيانات بشكل غير متزامن
                await _context.SaveChangesAsync();

                // 4. التوجيه إلى صفحة الملف الشخصي بعد التعديل بنجاح (عدلي اسم الـ Action حسب ما لديك)
                return RedirectToAction("Profile");
            }
            catch (DbUpdateConcurrencyException)
            {
                // معالجة إذا كان الكيان قد حذف
                if (!_context.Users.Any(e => e.User_ID == model.User_ID))
                {
                    return NotFound();
                }
                throw;
            }
            catch (Exception ex)
            {
                // معالجة أخطاء الحفظ الأخرى
                ModelState.AddModelError("", "حدث خطأ أثناء حفظ البيانات: " + ex.Message);
            }
        }

        // 5. إذا كانت البيانات غير صالحة أو حدث خطأ، أعيدي عرض النموذج
        return View(model);
    }
}
