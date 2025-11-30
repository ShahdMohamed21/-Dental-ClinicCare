using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Final_project.Models
{
    public class AppointmentService
    {
        [Key]
        public int AppService_ID { get; set; }

        
        [ForeignKey("Appointment")]
        public int App_ID { get; set; }
        public Appointments Appointment { get; set; }


        [ForeignKey("Service")]
        public int Service_ID { get; set; }
        public Service Service { get; set; }
    }
}
