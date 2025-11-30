using Final_project.Data;
using Final_project.Data.Services;
using Final_project.Data.Services_Control;
using Final_project.Services;
using Microsoft.AspNetCore.Authentication.Cookies; // ✅ مكتبة مهمة
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Text;

namespace Final_project
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ✅ 1. Add services to the container
            builder.Services.AddControllersWithViews();

            // ✅ 2. Database connection
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("conString")));

            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<PatientService>();
            builder.Services.AddScoped<NurseService>();
            builder.Services.AddScoped<StaffService>();
            builder.Services.AddScoped<ServiceService>();
            builder.Services.AddScoped<AppointmentService>(); //service
            builder.Services.AddScoped<FeedbackService>(); //service
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<PackageService>();


            // 🚀 التعديل النهائي: تهيئة Cookie Authentication (بدلاً من JWT Bearer)
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login"; 
                    options.ExpireTimeSpan = TimeSpan.FromHours(2);
                    options.SlidingExpiration = true;

                });
            

            // ✅ 5. Build app
            var app = builder.Build();

            // ✅ 6. Configure HTTP Request Pipeline
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // الترتيب الصحيح: UseAuthentication قبل UseAuthorization
            app.UseAuthentication();
            app.UseAuthorization();

            // ✅ Default route
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=HomePage}/{id?}");

            app.Run();
        }
    }
}