using internetprogramciligi1.Data;
using internetprogramciligi1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace internetprogramciligi1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class LessonController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LessonController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Hangi kursa ders ekleyeceğiz?
        public IActionResult Create(int courseId)
        {
            // O kursu bulalım ki başlığını görelim
            var course = _context.Courses.Find(courseId);
            if (course == null) return NotFound();

            ViewBag.CourseName = course.Title;

            // Modeli oluştururken CourseId'yi içine koyalım
            var lesson = new Lesson { CourseId = courseId };
            return View(lesson);
        }

        [HttpPost]
        public IActionResult Create(Lesson lesson)
        {
            if (ModelState.IsValid)
            {
                _context.Lessons.Add(lesson);
                _context.SaveChanges();

                // Kaydettikten sonra Kurs listesine dönelim (veya yine eklemeye)
                return RedirectToAction("List", "Course");
            }
            return View(lesson);
        }
    }
}