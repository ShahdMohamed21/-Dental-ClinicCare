using Final_project.Data.ViewModels;
using Final_project.Models;

namespace Final_project.Data.Services
{
    public class StaffService
    {
        public StaffService(AppDbContext context)
        {
            _context = context;
        }
        private readonly AppDbContext _context;
        public IEnumerable<Staff> GetAll()
        {
            return _context.Staffs.ToList();
        }

        public Staff? GetById(int id)
        {
            return _context.Staffs.Find(id);
        }

        public Staff Create(Staff model)
        {
            var staff = new Staff
            {
                Name = model.Name,
                Address = model.Address,
                Phone = model.Phone,
                Role = model.Role
            };

            _context.Staffs.Add(staff);
            _context.SaveChanges();
            return staff;
        }

        public Staff? Update(int id, Staff model)
        {
            var staff = _context.Staffs.Find(id);
            if (staff == null)
                return null;

            staff.Name = model.Name;
            staff.Address = model.Address;
            staff.Phone = model.Phone;
            staff.Role = model.Role;
            _context.SaveChanges();
            return staff;
        }

        public bool Delete(int id)
        {
            var staff = _context.Staffs.Find(id);
            if (staff == null) return false;

            _context.Staffs.Remove(staff);
            _context.SaveChanges();
            return true;
        }

    }
}