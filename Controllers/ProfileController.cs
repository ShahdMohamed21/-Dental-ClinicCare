using Final_project.Data;
using Final_project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System; // تأكد من وجود هذه المكتبة للاستثناءات
using System.Threading.Tasks;
using System.Linq;

public class ProfileController : Controller
{
    private readonly AppDbContext _context;

    public ProfileController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// دالة مساعدة للعثور على المستخدم المسؤول (Admin) في قاعدة البيانات.
    /// </summary>
    private async Task<User> GetAdminUserAsync()
    {
        var admin = await _context.Users
            .FirstOrDefaultAsync(u => u.Role != null && u.Role.ToLower() == "admin");
        return admin;
    }

    // عرض الملف الشخصي
    public async Task<IActionResult> Index()
    {
        var admin = await GetAdminUserAsync();
        if (admin == null)
        {
            TempData["Error"] = "لم يتم العثور على حساب مسؤول (Admin)!";
            return RedirectToAction("Index", "Home");
        }
        return View(admin);
    }


}