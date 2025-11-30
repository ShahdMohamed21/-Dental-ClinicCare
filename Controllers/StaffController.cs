using Final_project.Data.Services;
using Final_project.Models;
using Microsoft.AspNetCore.Mvc;

public class StaffController : Controller
{
    private readonly StaffService _staffService;

    public StaffController(StaffService staffService)
    {
        _staffService = staffService;
    }

    // GET: /Staff
    public IActionResult Index()
    {
        var staffList = _staffService.GetAll(); // IEnumerable<Staff>
        return View(staffList);
    }

    // GET: /Staff/Create
    // GET: /Staff/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: /Staff/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Staff model)
    {
        if (!ModelState.IsValid)
            return View(model);
        _staffService.Create(model);
        TempData["Success"] = "Staff added successfully ";
        return RedirectToAction("Index");
    }

    // GET: /Staff/Edit/5
    public IActionResult Edit(int id)
    {
        var staff = _staffService.GetById(id);
        if (staff == null) return NotFound();
        return View(staff);
    }

    // POST: /Staff/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Staff model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var updated = _staffService.Update(id, model);
        if (updated == null)
            return NotFound();

        TempData["Info"] = "Staff updated successfully";
    
        return RedirectToAction("Index");
    }

    // GET: /Staff/Delete/5
    public IActionResult Delete(int id)
    {
        var staff = _staffService.GetById(id);
        if (staff == null) return NotFound();
        return View(staff);
    }

    // POST: /Staff/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var deleted = _staffService.Delete(id);
        if (!deleted) return NotFound();

        TempData["Delete"] = "Staff deleted successfully";
        return RedirectToAction("Index");
    }

    // GET: /Staff/Details/5
    public IActionResult Details(int id)
    {
        var staff = _staffService.GetById(id);
        if (staff == null) return NotFound();
        return View(staff);
    }
}