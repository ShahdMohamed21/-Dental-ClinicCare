using Final_project.Data;
using Final_project.Models;
using Microsoft.EntityFrameworkCore;

namespace Final_project.Services
{
    public class PatientService
    {
        private readonly AppDbContext _db;

        public PatientService(AppDbContext db)
        {
            _db = db;
        }
        /////// Get All Patients
        public async Task<(bool Success, string Message, List<Patient>? Patients)> GetAllPatients()
        {
            try
            {
                var patients = await _db.Patients
                    .Include(p => p.Appointments)
                    .ToListAsync();

                return (true, "Patients retrieved successfully.", patients);
            }
            catch (Exception ex)
            {
                return (false, $"Error retrieving patients: {ex.Message}", null);
            }
        }
        //// Get Patient By ID
        public async Task<(bool Success, string Message, Patient? Patient)> GetPatientById(int id)
        {
            try
            {
                var patient = await _db.Patients
                    .Include(p => p.Appointments)
                    .ThenInclude(a => a.Doctor)
                    .FirstOrDefaultAsync(p => p.Patient_ID == id);

                if (patient == null)
                    return (false, "Patient not found.", null);

                return (true, "Patient retrieved successfully.", patient);
            }
            catch (Exception ex)
            {
                return (false, $"Error retrieving patient: {ex.Message}", null);
            }
        }

        // Get patients by name
        public async Task<(bool Success, string Message, List<Patient>? Patients)> GetPatientsByName(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    return (false, "Search name cannot be empty.", null);

                var searchTerm = name.Trim().ToLower();
                var result = await _db.Patients
                    .Where(p => p.fullName != null && p.fullName.ToLower().Contains(searchTerm))
                    .ToListAsync();

                if (!result.Any())
                    return (false, "No patients found matching the search term.", null);

                return (true, "Patients retrieved successfully.", result);
            }
            catch (Exception ex)
            {
                return (false, $"Error searching patients: {ex.Message}", null);
            }
        }

        // Add new patient
        public async Task<(bool Success, string Message, Patient? Patient)> AddPatient(Patient patient)
        {
            try
            {
                // Validation
                if (string.IsNullOrWhiteSpace(patient.fullName) ||
                    string.IsNullOrWhiteSpace(patient.Email) ||
                    string.IsNullOrWhiteSpace(patient.Phone))
                {
                    return (false, "Name, email, and phone are required.", null);
                }

                // Check if email exists
                var existingPatient = await _db.Patients.FirstOrDefaultAsync(p => p.Email == patient.Email);
                if (existingPatient != null)
                {
                    return (false, "A patient with this email already exists.", null);
                }

                _db.Patients.Add(patient);
                await _db.SaveChangesAsync();
                return (true, "Patient added successfully.", patient);
            }
            catch (Exception ex)
            {
                return (false, $"Error adding patient: {ex.Message}", null);
            }
        }

        //  Update patient
        public async Task<(bool Success, string Message)> UpdatePatient(int id, Patient updated)
        {
            try
            {
                var patient = await _db.Patients.FindAsync(id);
                if (patient == null)
                    return (false, "Patient not found.");

                if (string.IsNullOrWhiteSpace(updated.fullName) ||
                    string.IsNullOrWhiteSpace(updated.Email))
                {
                    return (false, "Full name and email cannot be empty.");
                }

                // Update fields
                patient.fullName = updated.fullName;
                patient.Gender = updated.Gender;
                patient.Phone = updated.Phone;
                patient.Email = updated.Email;
                patient.Address = updated.Address;

                await _db.SaveChangesAsync();
                return (true, "Patient updated successfully.");
            }
            catch (Exception ex)
            {
                return (false, $"Error updating patient: {ex.Message}");
            }
        }

        //  Delete patient
        public async Task<(bool Success, string Message)> DeletePatient(int id)
        {
            try
            {
                var patient = await _db.Patients.FindAsync(id);
                if (patient == null)
                    return (false, "Patient not found.");

                _db.Patients.Remove(patient);
                await _db.SaveChangesAsync();
                return (true, "Patient deleted successfully.");
            }
            catch (Exception ex)
            {
                return (false, $"Error deleting patient: {ex.Message}");
            }
        }
    }
}