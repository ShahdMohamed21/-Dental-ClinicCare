using Final_project.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;

namespace Final_project.Data.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // ✅ تسجيل مستخدم جديد فقط (من غير إضافته كمريض)
        public async Task<bool> RegisterAsync(string fullName, string gender, string phone, string email, string address, string password)
        {
            try
            {
                // تأكد إن الإيميل مش مستخدم قبل كده
                email = email.Trim().ToLower();
                if (await _context.Users.AnyAsync(u => u.Email == email))
                    return false;

                // نضيف المستخدم في جدول Users
                var newUser = new User
                {
                    FullName = fullName.Trim(),
                    Gender = gender.Trim(),
                    Phone = phone.Trim(),
                    Email = email,
                    Address = address.Trim(),
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                    Role = "User"
                };

                await _context.Users.AddAsync(newUser);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Register Error: {ex.Message}");
                return false;
            }
        }

        // ✅ تسجيل الدخول
        public async Task<ClaimsPrincipal?> LoginAsync(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());

            if (user == null)
            {
                Console.WriteLine("❌ المستخدم غير موجود");
                return null;
            }

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);

            if (!isPasswordValid)
            {
                Console.WriteLine("❌ كلمة المرور غير صحيحة");
                return null;
            }

            // ✅ إنشاء المطالبات (Claims)
            var claims = new List<Claim>
            {
                // ClaimTypes.NameIdentifier هو الـ User_ID
                new Claim(ClaimTypes.NameIdentifier, user.User_ID.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role), // 👈 الصلاحية هنا
                new Claim("User_ID", user.User_ID.ToString())
            };

            // ❌ تم حذف كود توليد التوكن JWT
            // 🚀 نُرجع كائن المطالبات جاهزاً للاستخدام في SignInAsync
            return new ClaimsPrincipal(new ClaimsIdentity(claims, "CustomAuth"));
        }
    }
}


