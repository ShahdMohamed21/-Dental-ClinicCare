using Final_project.Data.Services;
using Final_project.Data.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Final_project.Controllers
{
    public class AccountController : Controller
    {
        private readonly AuthService _authService;

        public AccountController(AuthService authService)
        {
            _authService = authService;
        }

        // ... (Register Actions - بدون تغيير في المسار) ...

        [HttpGet]
        public IActionResult Register()
        {
            return View("~/Views/Account/Register.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var result = await _authService.RegisterAsync(
                    model.FullName, model.Gender, model.Phone,
                    model.Email, model.Address, model.Password
                );

                if (!result)
                {
                    ModelState.AddModelError("", "البريد الإلكتروني مستخدم من قبل أو حدث خطأ أثناء التسجيل ❌");
                    return View(model);
                }

                TempData["RegisterSuccess"] = "تم التسجيل بنجاح 🎉 يمكنك تسجيل الدخول الآن";
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"حدث خطأ غير متوقع ❌: {ex.Message}");
                return View(model);
            }
        }

        // ✅ عرض صفحة تسجيل الدخول
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        // في ملف AccountController.cs
        // ... (أسفل دالة Login)

        // ✅ دالة تسجيل الخروج لإنهاء الجلسة
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // إنهاء جلسة التوثيق وحذف الكوكي
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // التوجيه إلى الصفحة الرئيسية
            return RedirectToAction("HomePage", "Home");
        }

        // 🚀 تنفيذ عملية تسجيل الدخول باستخدام Claims
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                // 🚀 الآن نتوقع ClaimsPrincipal بدلاً من string token
                var claimsPrincipal = await _authService.LoginAsync(model.Email, model.Password);

                if (claimsPrincipal == null)
                {
                    ModelState.AddModelError("", "البريد الإلكتروني أو كلمة المرور غير صحيحة ❌");
                    return View(model);
                }

                // 🌟 تسجيل الدخول الرسمي في ASP.NET Core
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    claimsPrincipal,
                    new AuthenticationProperties
                    {
                        IsPersistent = true, // تذكرني
                        ExpiresUtc = DateTimeOffset.UtcNow.AddHours(2)
                    });

                // 🚀 التعديل 2: استخدام مفتاح TempData مختلف لرسالة نجاح تسجيل الدخول
                TempData["LoginSuccess"] = "تم تسجيل الدخول بنجاح ✅";
                return RedirectToAction("HomePage", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"حدث خطأ أثناء تسجيل الدخول ❌: {ex.Message}");
                return View(model);
            }
        }
    }
}




