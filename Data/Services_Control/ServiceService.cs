using Final_project.Data;
using Final_project.Models;
using Microsoft.EntityFrameworkCore;

namespace Final_project.Services
{
    public class ServiceService
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public ServiceService(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        private async Task<string?> SaveImage(IFormFile? file)
        {
            if (file == null || file.Length == 0) return null;

            try
            {
                string folder = Path.Combine(_env.WebRootPath, "uploads", "services");
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                string filePath = Path.Combine(folder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return $"services/{fileName}";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving image: {ex.Message}");
                return null;
            }
        }

        // Get all services
        public async Task<(bool Success, string Message, List<Service>? Data)> GetAllServices()
        {
            try
            {
                var services = await _db.Services.ToListAsync();
                return (true, "Services retrieved successfully.", services);
            }
            catch (Exception ex)
            {
                return (false, $"Error retrieving services: {ex.Message}", null);
            }
        }

        // Get service by ID
        public async Task<(bool Success, string Message, Service? Data)> GetServiceById(int id)
        {
            try
            {
                var service = await _db.Services.FindAsync(id);
                if (service == null)
                    return (false, "Service not found.", null);

                return (true, "Service retrieved successfully.", service);
            }
            catch (Exception ex)
            {
                return (false, $"Error retrieving service: {ex.Message}", null);
            }
        }

        // Add new service
        public async Task<(bool Success, string Message, Service? Data)> AddService(Service service)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(service.Name) || service.Price <= 0)
                    return (false, "Service name and valid price are required.", null);

                // Handle image upload
                if (service.ImageFile != null && service.ImageFile.Length > 0)
                {
                    var imagePath = await SaveImage(service.ImageFile);
                    if (!string.IsNullOrEmpty(imagePath))
                        service.ImagePath = imagePath;
                }

                _db.Services.Add(service);
                await _db.SaveChangesAsync();

                return (true, "Service added successfully.", service);
            }
            catch (Exception ex)
            {
                return (false, $"Error adding service: {ex.Message}", null);
            }
        }

        // Update service
        public async Task<(bool Success, string Message)> UpdateService(int id, Service updated)
        {
            try
            {
                var service = await _db.Services.FindAsync(id);
                if (service == null)
                    return (false, "Service not found.");

                if (string.IsNullOrWhiteSpace(updated.Name) || updated.Price <= 0)
                    return (false, "Service name and valid price are required.");

                service.Name = updated.Name;
                service.Description = updated.Description;
                service.Price = updated.Price;

                // Handle image upload if new image provided
                if (updated.ImageFile != null && updated.ImageFile.Length > 0)
                {
                    var imagePath = await SaveImage(updated.ImageFile);
                    if (!string.IsNullOrEmpty(imagePath))
                        service.ImagePath = imagePath;
                }

                await _db.SaveChangesAsync();
                return (true, "Service updated successfully.");
            }
            catch (Exception ex)
            {
                return (false, $"Error updating service: {ex.Message}");
            }
        }

        // Delete service
        public async Task<(bool Success, string Message)> DeleteService(int id)
        {
            try
            {
                var service = await _db.Services.FindAsync(id);
                if (service == null)
                    return (false, "Service not found.");

                _db.Services.Remove(service);
                await _db.SaveChangesAsync();
                return (true, "Service deleted successfully.");
            }
            catch (Exception ex)
            {
                return (false, $"Error deleting service: {ex.Message}");
            }
        }
    }
}