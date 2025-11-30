using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Final_project.Models
{
    public class AppointmentPackages
    {
        [Key]
        public int AppService_ID { get; set; }


        [ForeignKey("AppointmentsP")]
        public int App_ID { get; set; }
        public AppointmentsP AppointmentsP { get; set; }


        [ForeignKey("Package")]
        public int Package_ID { get; set; }
        public Package Package { get; set; }
    }
}