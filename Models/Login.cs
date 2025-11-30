using System.ComponentModel.DataAnnotations;

namespace Final_project.Models
{
    public class Login
    {
        [Key]
        public int User_ID { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }
    }
}
