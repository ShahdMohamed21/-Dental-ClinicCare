using Final_project.Models;
using Final_project.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final_project.Controllers
{
    public class PatientController : Controller
    {
        private readonly PatientService _patientService;

        public PatientController(PatientService patientService)
        {
            _patientService = patientService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _patientService.GetAllPatients();

            if (!result.Success || result.Patients == null)
            {
                ViewBag.ErrorMessage = result.Message;
                return View(new List<Patient>());
            }

            var patients = result.Patients;

            // الإحصائيات
            // الإحصائيات
            ViewBag.Total = patients.Count;
            ViewBag.Male = patients.Count(p => p.Gender == "Male" || p.Gender == "ذكر");
            ViewBag.Female = patients.Count(p => p.Gender == "Female" || p.Gender == "أنثى");

            ViewBag.SuccessMessage = result.Message;
            return View(patients);
        }

        //  Search Patient by Name (GET)
        [HttpGet]
        public IActionResult Search()
        {
            return View();
        }

        //  Search Patient by Name (POST)
        [HttpPost]
        public async Task<IActionResult> Search(string name)
        {
            var result = await _patientService.GetPatientsByName(name);

            if (!result.Success || result.Patients == null)
            {
                ViewBag.Message = result.Message;
                return View(new List<Patient>());
            }

            ViewBag.Message = result.Message;
            ViewBag.SearchTerm = name;
            return View(result.Patients);
        }

        //  Add New Patient (GET)
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        //  Add New Patient (POST)
        [HttpPost]
        public async Task<IActionResult> Create(Patient patient)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);
                ViewBag.ErrorMessage = "Validation failed: " + string.Join("; ", errors);
               
                return View(patient);
            }

            var result = await _patientService.AddPatient(patient);

            if (!result.Success)
            {
                TempData["Error"] = "Please Check Again";
                ViewBag.ErrorMessage = result.Message;
                return View(patient);
            }
            TempData["Success"] = "Patient Added Successfully";
            return RedirectToAction(nameof(Index));
        }

        //  Edit Patient (GET)
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _patientService.GetPatientById(id);

            if (!result.Success || result.Patient == null)
            {
                return NotFound(result.Message);
            }

            return View(result.Patient);
        }

        // Edit Patient (POST)
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Patient updated)
        {
            var result = await _patientService.UpdatePatient(id, updated);

            if (!result.Success)
            {
                ViewBag.ErrorMessage = result.Message;
                return View(updated);
            }
            TempData["Info"] = "Patient Updated Successfully ";

        
            return RedirectToAction(nameof(Index));
        }

        // Delete Patient
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _patientService.DeletePatient(id);
            TempData["Delete"] = "Patient Deleted Successfully";
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var result = await _patientService.GetPatientById(id);

            if (!result.Success || result.Patient == null)
            {
                return NotFound(result.Message);
            }

            return View(result.Patient);
        }
       
    }
}