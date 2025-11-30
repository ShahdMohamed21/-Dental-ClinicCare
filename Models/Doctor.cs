using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Final_project.Models
{
    public class Doctor
    {
        [Key]
        public int Doctor_ID { get; set; }

        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Specialty { get; set; }

        // Navigation Properties

        [ValidateNever]
        public ICollection<Appointments> Appointments { get; set; }
    }
}