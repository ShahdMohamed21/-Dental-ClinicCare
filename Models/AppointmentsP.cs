using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Final_project.Models
{
    public class AppointmentsP
    {
        [Key]
        public int App_ID { get; set; }

        [ForeignKey("Patient")]
        public int Patient_ID { get; set; }
        public Patient? Patient { get; set; }
        [ForeignKey("Doctor")]
        public int Doctor_ID { get; set; }
        public Doctor? Doctor { get; set; }

        public DateTime AppointmentDate { get; set; }
        public TimeSpan AppointmentTime { get; set; }
        public string? PaymentMethod { get; set; }
        public string? Notes { get; set; }
        public string Status { get; set; } = "Pending";

        public ICollection<AppointmentPackages>? AppointmentPackages { get; set; }
    }
}