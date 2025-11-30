using Final_project.Data;
using Final_project.Models;

public class FeedbackService
{
    private readonly AppDbContext _context;

    public FeedbackService(AppDbContext context)
    {
        _context = context;
    }

    public void AddFeedback(Feedback feedback)
    {
        _context.Feedbacks.Add(feedback);
        _context.SaveChanges();
    }

    public List<Feedback> GetAllFeedbacks()
    {
        return _context.Feedbacks.OrderByDescending(f => f.Date).ToList();
    }
}
