using System.ComponentModel.DataAnnotations;

namespace Final_project.Models
{
    public class PackageBookingViewModel
    {
        [Required(ErrorMessage = "الاسم الأول مطلوب.")]
        [Display(Name = "الاسم الأول")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "اسم العائلة مطلوب.")]
        [Display(Name = "اسم العائلة")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "البريد الإلكتروني مطلوب.")]
        [EmailAddress(ErrorMessage = "الرجاء إدخال بريد إلكتروني صحيح.")]
        [Display(Name = "البريد الإلكتروني")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "رقم الهاتف مطلوب.")]
        [Phone(ErrorMessage = "الرجاء إدخال رقم هاتف صحيح.")]
        [Display(Name = "رقم الهاتف")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "السن مطلوب.")]
        [Range(1, 120, ErrorMessage = "السن يجب أن يكون بين 1 و 120.")]
        [Display(Name = "السن")]
        public int Age { get; set; }

        [Required(ErrorMessage = "النوع مطلوب.")]
        [Display(Name = "النوع")]
        public string Gender { get; set; } = string.Empty;

        [Required(ErrorMessage = "تاريخ الحجز مطلوب.")]
        [Display(Name = "تاريخ الحجز")]
        [DataType(DataType.Date)]
        public DateOnly AppointmentDate { get; set; }

        [Required(ErrorMessage = "وقت الحجز مطلوب.")]
        [Display(Name = "وقت الحجز")]
        [DataType(DataType.Time)]
        public TimeOnly AppointmentTime { get; set; }

        [Display(Name = "ملاحظات")]
        public string? Notes { get; set; }
        [Required(ErrorMessage = "الرجاء اختيار طريقة الدفع.")]
        [Display(Name = "طريقة الدفع")]
        public string PaymentMethod { get; set; } = string.Empty;

        // أهم فرق هنا 👇
        public List<int> SelectedPackages { get; set; } = new List<int>();
    }
}