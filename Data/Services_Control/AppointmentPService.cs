using Final_project.Models;
using Microsoft.EntityFrameworkCore;

namespace Final_project.Data.Services_Control
{
    public class AppointmentPService
    {
        private readonly AppDbContext _context;

        public AppointmentPService(AppDbContext context)
        {
            _context = context;
        }

        // =====================================
        // 1) Get All Package Appointments
        // =====================================
        public List<AppointmentsP> GetAllAppointments()
        {
            return _context.Set<AppointmentsP>()
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Include(a => a.AppointmentPackages)
                    .ThenInclude(ap => ap.Package)
                .OrderByDescending(a => a.AppointmentDate)
                .ToList();
        }

        // =====================================
        // 2) Add New Package Appointment
        // =====================================
        public void AddAppointment(AppointmentsP appointment)
        {
            appointment.Status = "Pending";
            _context.Set<AppointmentsP>().Add(appointment);
            _context.SaveChanges(); // VERY IMPORTANT so App_ID gets generated
        }

        // =====================================
        // 3) Get Appointment By ID
        // =====================================
        public AppointmentsP? GetAppointmentById(int id)
        {
            return _context.Set<AppointmentsP>()
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Include(a => a.AppointmentPackages)
                    .ThenInclude(ap => ap.Package)
                .FirstOrDefault(a => a.App_ID == id);
        }

        // =====================================
        // 4) Update Status (Confirmed / Pending / Cancelled)
        // =====================================
        public void UpdateAppointmentStatus(int id, string newStatus)
        {
            var appointment = _context.Set<AppointmentsP>().FirstOrDefault(a => a.App_ID == id);

            if (appointment != null)
            {
                appointment.Status = newStatus;
                _context.SaveChanges();
            }
        }
    }
}
