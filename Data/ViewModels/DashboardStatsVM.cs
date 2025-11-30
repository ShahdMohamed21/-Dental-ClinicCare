public class DashboardStatsVM
{
    public int TotalPatients { get; set; }
    public int TotalAppointments { get; set; }
    public int TotalUsers { get; set; } // بدل الإيرادات

    public int PatientsPercentage { get; set; }
    public int AppointmentsPercentage { get; set; }
    public int UsersPercentage { get; set; } // بدل RevenuePercentage
}
