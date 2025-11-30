using System.ComponentModel.DataAnnotations;

namespace Final_project.Data.ViewModels
{
    public class RegisterVM
    {
        [Display(Name = "الاسم الكامل")]
        [Required(ErrorMessage = "الاسم الكامل مطلوب 🛑")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "يجب أن يكون الاسم بين 3 و 100 حرف.")]
        public string FullName { get; set; }

        [Display(Name = "النوع")]
        [Required(ErrorMessage = "النوع (ذكر/أنثى) مطلوب 🛑")]
        [StringLength(10, ErrorMessage = "النوع يجب أن يكون نصاً قصيراً.")]
        public string Gender { get; set; }

        [Display(Name = "رقم الهاتف")]
        [Required(ErrorMessage = "رقم الهاتف مطلوب 🛑")]
        [Phone(ErrorMessage = "صيغة رقم الهاتف غير صحيحة.")]
        [RegularExpression(@"^01[0125]\d{8}$", ErrorMessage = "أدخل رقم هاتف مصري صحيح (11 رقم).")]
        public string Phone { get; set; }

        [Display(Name = "البريد الإلكتروني")]
        [Required(ErrorMessage = "البريد الإلكتروني مطلوب 🛑")]
        [EmailAddress(ErrorMessage = "صيغة البريد الإلكتروني غير صحيحة.")]
        public string Email { get; set; }

        [Display(Name = "العنوان")]
        [Required(ErrorMessage = "العنوان مطلوب 🛑")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "يجب أن يكون العنوان بين 5 و 200 حرف.")]
        public string Address { get; set; }

        [Display(Name = "كلمة المرور")]
        [Required(ErrorMessage = "كلمة المرور مطلوبة 🛑")]
        [DataType(DataType.Password)]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "يجب أن تكون كلمة المرور بين 6 و 30 حرف.")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,}$",
            ErrorMessage = "يجب أن تحتوي كلمة المرور على الأقل على حرف واحد (أو أكثر) ورقم واحد (أو أكثر).")]
        public string Password { get; set; }

        [Display(Name = "تأكيد كلمة المرور")]
        [Required(ErrorMessage = "تأكيد كلمة المرور مطلوب 🛑")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "كلمة المرور وتأكيدها غير متطابقتان.")]
        public string ConfirmPassword { get; set; }
    }
}