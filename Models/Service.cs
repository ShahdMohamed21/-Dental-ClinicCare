using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Final_project.Models
{
    public class Service
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Service_ID { get; set; }

        [Required(ErrorMessage = "Service name is required")]
        [StringLength(150, ErrorMessage = "Service name cannot exceed 150 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Price is required")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0, 999999.99, ErrorMessage = "Price must be a positive value")]
        public decimal Price { get; set; }

        // ✅ Store uploaded image file name/path in DB  
        public string? ImagePath { get; set; }

        // ✅ This will receive the uploaded file form form. Will NOT be in DB
        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        public string? Image { get; set; }
        // Navigation
        public ICollection<AppointmentService> AppointmentServices { get; set; }
            = new List<AppointmentService>();
    }
}