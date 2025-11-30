using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Final_project.Models
{
    public class Appointments
    {
        [Key]
        public int App_ID { get; set; }

        [ForeignKey("Patient")]
        [Required(ErrorMessage = "يجب اختيار المريض")]
        public int Patient_ID { get; set; }
        public Patient? Patient { get; set; }

        [ForeignKey("Doctor")]
        [Required(ErrorMessage = "يجب اختيار الطبيب")]
        public int Doctor_ID { get; set; }
        public Doctor? Doctor { get; set; }

        [Required(ErrorMessage = "يجب تحديد تاريخ الموعد")]
        [DataType(DataType.Date)]
        public DateTime AppointmentDate { get; set; }

        [Required(ErrorMessage = "يجب تحديد وقت الموعد")]
        [DataType(DataType.Time)]
        public TimeSpan AppointmentTime { get; set; }

        [Required(ErrorMessage = "يجب اختيار طريقة الدفع")]
        public string? PaymentMethod { get; set; }

        [StringLength(500, ErrorMessage = "الملاحظات لا يمكن أن تزيد عن 500 حرف")]
        public string? Notes { get; set; }

        public string Status { get; set; } = "Pending";

        [Required(ErrorMessage = "يجب اختيار خدمة واحدة على الأقل")]
        public ICollection<AppointmentService>? AppointmentServices { get; set; }
    }
}
