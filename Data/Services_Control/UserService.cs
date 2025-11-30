using System.Collections.Generic;
using System.Linq;
using Final_project.Data;
using Final_project.Models;

public class UserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    public List<User> GetAllUsers()
    {
        return _context.Users.ToList();
    }

    public User GetUserById(int id)
    {
        return _context.Users.FirstOrDefault(u => u.User_ID == id);
    }

    public void DeleteUser(int id)
    {
        var user = _context.Users.FirstOrDefault(u => u.User_ID == id);
        if (user != null)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }
    }
}
