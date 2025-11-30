using System.ComponentModel.DataAnnotations;

namespace Final_project.Models
{
    public class Nurses
    {
        [Key]
        public int Nurse_ID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }
}
