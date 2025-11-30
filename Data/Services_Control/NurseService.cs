using Final_project.Data.ViewModels;
using Final_project.Models;

namespace Final_project.Data.Services
{
    public class NurseService
    {
        private readonly AppDbContext _context;

        public NurseService(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Nurses> GetAll()
        {
            return _context.Nurses.ToList();
        }

        public Nurses? GetById(int id)
        {
            return _context.Nurses.Find(id);
        }

        public Nurses Create(Nurses model)
        {
            var nurse = new Nurses
            {
                Name = model.Name,
                Address = model.Address,
                Phone = model.Phone
            };

            _context.Nurses.Add(nurse);
            _context.SaveChanges();
            return nurse;
        }

        public Nurses? Update(int id, Nurses model)
        {
            var nurse = _context.Nurses.Find(id);
            if (nurse == null)
                return null;

            nurse.Name = model.Name;
            nurse.Address = model.Address;
            nurse.Phone = model.Phone;
            _context.SaveChanges();
            return nurse;
        }

        public bool Delete(int id)
        {
            var nurse = _context.Nurses.Find(id);
            if (nurse == null)
                return false;

            _context.Nurses.Remove(nurse);
            _context.SaveChanges();
            return true;
        }
    }
}