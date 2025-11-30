using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Final_project.Models
{
    public class Records
    {
        [Key]
        public int RecordId { get; set; }

        [ForeignKey("Appointment")]
        public int AppointmentId { get; set; }
        public Appointments Appointment { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }
    }
}
