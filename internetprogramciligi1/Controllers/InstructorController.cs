using internetprogramciligi1.Data;
using internetprogramciligi1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace internetprogramciligi1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class InstructorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InstructorController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var instructors = _context.Instructors.ToList();
            return View(instructors);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Instructor instructor)
        {
            if (ModelState.IsValid)
            {
                _context.Instructors.Add(instructor);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(instructor);
        }

        public IActionResult Delete(int id)
        {
            var instructor = _context.Instructors.Find(id);
            if (instructor != null)
            {
                _context.Instructors.Remove(instructor);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteAjax(int id)
        {
            var instructor = _context.Instructors.Find(id);
            if (instructor != null)
            {
                _context.Instructors.Remove(instructor);
                _context.SaveChanges();
            }
            return Json(new { success = true });
        }

        // --- DÜZELTİLEN KISIM (Repository yerine _context kullanıldı) ---

        [HttpGet]
        public IActionResult Edit(int id)
        {
            // _instructorRepository yerine _context kullanıyoruz
            var instructor = _context.Instructors.Find(id);
            if (instructor == null) return NotFound();
            return View(instructor);
        }

        [HttpPost]
        public IActionResult Edit(Instructor instructor)
        {
            if (ModelState.IsValid)
            {
                // _instructorRepository yerine _context kullanıyoruz
                _context.Instructors.Update(instructor);
                _context.SaveChanges(); // Kaydetmeyi unutmuyoruz
                return RedirectToAction("Index");
            }
            return View(instructor);
        }
    }
}