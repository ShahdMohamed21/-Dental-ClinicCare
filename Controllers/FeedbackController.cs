// FeedbackController.cs
using Final_project.Data;
using Final_project.Models;
using Microsoft.AspNetCore.Mvc;

public class FeedbackController : Controller
{
    private readonly AppDbContext _context;

    public FeedbackController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Feedback model)
    {
        if (ModelState.IsValid)
        {
            _context.Feedbacks.Add(model);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "تم الإرسال بنجاح!";
            return RedirectToAction("Create"); // تعيد الصفحة نفسها
        }
        return View(model);
    }

    // GET: Feedback/AdminList
    [HttpGet]
    public IActionResult AdminList()
    {
        // جلب كل التعليقات من قاعدة البيانات، ترتيبها من الأحدث للأقدم
        var feedbacks = _context.Feedbacks.OrderByDescending(f => f.Date).ToList();
        return View(feedbacks);
    }

}
