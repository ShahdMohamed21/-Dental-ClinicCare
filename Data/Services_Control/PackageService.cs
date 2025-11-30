using Final_project.Data;
using Final_project.Models;
using Microsoft.EntityFrameworkCore;

namespace Final_project.Services
{
    public class PackageService
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public PackageService(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        // ===================== Save Image =====================
        private async Task<string?> SaveImage(IFormFile? file)
        {
            if (file == null || file.Length == 0)
                return null;

            try
            {
                string folder = Path.Combine(_env.WebRootPath, "uploads", "packages");

                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                string filePath = Path.Combine(folder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return $"packages/{fileName}";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving image: {ex.Message}");
                return null;
            }
        }

        // ===================== Get All Packages =====================
        public async Task<(bool Success, string Message, List<Package>? Data)> GetAllPackages()
        {
            try
            {
                var packages = await _db.Packages.ToListAsync();
                return (true, "Packages retrieved successfully.", packages);
            }
            catch (Exception ex)
            {
                return (false, $"Error retrieving packages: {ex.Message}", null);
            }
        }

        // ===================== Get Package by ID =====================
        public async Task<(bool Success, string Message, Package? Data)> GetPackageById(int id)
        {
            try
            {
                var package = await _db.Packages.FindAsync(id);
                if (package == null)
                    return (false, "Package not found.", null);

                return (true, "Package retrieved successfully.", package);
            }
            catch (Exception ex)
            {
                return (false, $"Error retrieving package: {ex.Message}", null);
            }
        }

        // ===================== Add Package =====================
        public async Task<(bool Success, string Message, Package? Data)> AddPackage(Package package)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(package.Name) || package.Price <= 0)
                    return (false, "Package name and a valid price are required.", null);

                if (package.ImageFile != null && package.ImageFile.Length > 0)
                {
                    var imagePath = await SaveImage(package.ImageFile);
                    if (!string.IsNullOrWhiteSpace(imagePath))
                        package.ImagePath = imagePath;
                }

                _db.Packages.Add(package);
                await _db.SaveChangesAsync();

                return (true, "Package added successfully.", package);
            }
            catch (Exception ex)
            {
                return (false, $"Error adding package: {ex.Message}", null);
            }
        }

        // ===================== Update Package =====================
        public async Task<(bool Success, string Message)> UpdatePackage(int id, Package updated)
        {
            try
            {
                var package = await _db.Packages.FindAsync(id);
                if (package == null)
                    return (false, "Package not found.");

                if (string.IsNullOrWhiteSpace(updated.Name) || updated.Price <= 0)
                    return (false, "Package name and valid price are required.");

                package.Name = updated.Name;
                package.Description = updated.Description;
                package.Price = updated.Price;

                // If new image uploaded, replace old one
                if (updated.ImageFile != null && updated.ImageFile.Length > 0)
                {
                    var imagePath = await SaveImage(updated.ImageFile);
                    if (!string.IsNullOrWhiteSpace(imagePath))
                        package.ImagePath = imagePath;
                }

                await _db.SaveChangesAsync();
                return (true, "Package updated successfully.");
            }
            catch (Exception ex)
            {
                return (false, $"Error updating package: {ex.Message}");
            }
        }

        // ===================== Delete Package =====================
        public async Task<(bool Success, string Message)> DeletePackage(int id)
        {
            try
            {
                var package = await _db.Packages.FindAsync(id);
                if (package == null)
                    return (false, "Package not found.");

                _db.Packages.Remove(package);
                await _db.SaveChangesAsync();

                return (true, "Package deleted successfully.");
            }
            catch (Exception ex)
            {
                return (false, $"Error deleting package: {ex.Message}");
            }
        }
    }
}