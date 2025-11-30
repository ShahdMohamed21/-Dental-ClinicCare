using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Final_project.Models
{
    public class Patient
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Patient_ID { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 3)]
        public string fullName { get; set; }

        [Required]
        [StringLength(10)]
        public string Gender { get; set; }

        [Required]
        [Phone]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Phone number must be exactly 11 digits")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Phone number must be exactly 11 digits")]
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(256)]
        public string Email { get; set; }

        [Required]
        [StringLength(500)]
        public string Address { get; set; }

        // 🔥 علاقة: المريض تابع ليوزر اختياري (nullable لو مش لازم كل مريض له يوزر)
        [ForeignKey("User")]
        public int? User_ID { get; set; }
        public User? User { get; set; }


        // المواعيد
        public ICollection<Appointments> Appointments { get; set; } = new List<Appointments>();
        public ICollection<AppointmentsP> AppointmentsP { get; set; } = new List<AppointmentsP>();
    }
}
