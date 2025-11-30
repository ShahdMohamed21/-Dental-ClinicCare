using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
namespace Final_project.Models
{
    public class User
    {
        [Key]
        public int User_ID { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string FullName { get; set; }

        [Required]
        [StringLength(10)]
        public string Gender { get; set; }

        [Required]
        [Phone]
        [StringLength(20)]
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(256)]
        public string Email { get; set; }

        [Required]
        [StringLength(500)]
        public string Address { get; set; }

        [Required]
        [StringLength(255)]
        [ValidateNever]
        public string PasswordHash { get; set; }

        [StringLength(20)]
        public string Role { get; set; } = "User";

        // Relation: User → Patients (One To Many)
        public ICollection<Patient> Patients { get; set; } = new List<Patient>();
    }
}
