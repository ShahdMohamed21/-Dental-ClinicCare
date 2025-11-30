using System.ComponentModel.DataAnnotations;

namespace Final_project.Models
{
    public class Staff
    {
        [Key]
        public int Staff_ID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string? Role { get; set; }

    }
}
