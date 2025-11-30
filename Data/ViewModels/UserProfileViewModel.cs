namespace Final_project.ViewModels
{
    using Final_project.Models;
    using System.Collections.Generic;

    public class UserProfileViewModel
    {
        public User User { get; set; }
        public List<Appointments> Appointments { get; set; } = new List<Appointments>();
    }
}
