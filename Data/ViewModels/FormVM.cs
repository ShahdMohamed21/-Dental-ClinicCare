using System.ComponentModel.DataAnnotations;

namespace Final_project.Data.ViewModels
{
    public class FormVM
    {
            [Required]
            public DateTime AppointmentDate { get; set; }

            [Required]
            public TimeSpan AppointmentTime { get; set; }

            [Required]
            public int ServiceId { get; set; }

            public string PaymentMethod { get; set; }
 
    }
}
