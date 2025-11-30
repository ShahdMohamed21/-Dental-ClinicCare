namespace Final_project.Models
{
    public class Feedback
    {
        public int Id { get; set; }

        public string PatientName { get; set; }

        public string Comment { get; set; }

        public int Rating { get; set; } // من 1 لـ 5

        public DateTime Date { get; set; } = DateTime.Now;
    }

}
