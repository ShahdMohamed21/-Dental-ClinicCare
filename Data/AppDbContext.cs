using Final_project.Models;
using Microsoft.EntityFrameworkCore;

namespace Final_project.Data
{
    public class AppDbContext : DbContext
    {


        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<Nurses> Nurses { get; set; }
        public DbSet<Appointments> Appointments { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<AppointmentService> AppointmentServices { get; set; }
        public DbSet<Records> Records { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<AppointmentsP> AppointmentsP { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<AppointmentPackages> AppointmentPackages { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Patient>()
                .HasOne(p => p.User)
                .WithMany(u => u.Patients)
                .HasForeignKey(p => p.User_ID)
                .OnDelete(DeleteBehavior.SetNull);
        }





    }
}