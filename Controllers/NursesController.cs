using Final_project.Data.Services;
using Final_project.Data.ViewModels;
using Final_project.Models;
using Final_project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final_project.Controllers
{
    public class NursesController : Controller
    {
        private readonly NurseService _nurseService;

        public NursesController(NurseService nurseService)
        {
            _nurseService = nurseService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var nurses = _nurseService.GetAll(); // synchronous
            return View(nurses);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Nurses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Nurses model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _nurseService.Create(model);

            TempData["Success"] = "Nurse added successfully!";
            return RedirectToAction("Index");
        }

        // GET: /Nurses/Edit/5
        public IActionResult Edit(int id)
        {
            var nurse = _nurseService.GetById(id);
            if (nurse == null)
                return NotFound();

            return View(nurse);
        }

        // POST: /Nurses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Nurses model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var updated = _nurseService.Update(id, model);
            if (updated == null)
            {
                return NotFound();

                TempData["Error"] = "Nurse Does Not Found";
            }

           

            TempData["Info"] = "Nurse updated successfully!";
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult Details(int id)
        {
            var nurse = _nurseService.GetById(id);
            if (nurse == null)
                return NotFound();

            return View(nurse); // هيروح على Views/Nurses/Details.cshtml
        }


        // GET: /Nurses/Delete/5
        public IActionResult Delete(int id)
        {
            var nurse = _nurseService.GetById(id);
            if (nurse == null)
                return NotFound();

            return View(nurse);
        }

        // POST: /Nurses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var deleted = _nurseService.Delete(id);
            if (!deleted)
            {
                return NotFound();

                TempData["Error"] = "Nurse Does Not Found";
            }

            TempData["Delete"] = "Nurse deleted successfully!";
            return RedirectToAction("Index");
        }
    }
}