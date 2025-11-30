using Final_project.Models;
using Microsoft.EntityFrameworkCore;

namespace Final_project.Data.Services_Control
{
    public class AppointmentService
    {
        private readonly AppDbContext _context;
        public AppointmentService(AppDbContext context)
        {
            _context = context;
        }

        public List<Appointments> GetAllAppointments()
        {
            return _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Include(a => a.AppointmentServices)
                    .ThenInclude(asv => asv.Service)
                .OrderByDescending(a => a.AppointmentDate)
                .ToList();
        }

        public void AddAppointment(Appointments appointment)
        {
            appointment.Status = "Pending";
            _context.Appointments.Add(appointment);
            _context.SaveChanges(); // مهم جدًا عشان App_ID يتحدث
        }

        public Appointments GetAppointmentById(int id)
        {
            return _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Include(a => a.AppointmentServices)
                    .ThenInclude(asv => asv.Service)
                .FirstOrDefault(a => a.App_ID == id);
        }

        public void UpdateAppointmentStatus(int id, string newStatus)
        {
            var appointment = _context.Appointments.FirstOrDefault(a => a.App_ID == id);
            if (appointment != null)
            {
                appointment.Status = newStatus;
                _context.SaveChanges();
            }
        }
    }
}