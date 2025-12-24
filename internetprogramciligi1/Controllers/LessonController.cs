using internetprogramciligi1.Data;
using internetprogramciligi1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

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

        // --- DERS LİSTELEME ---
        public IActionResult List(int courseId)
        {
            // Sadece seçilen kursun derslerini getir
            var lessons = _context.Lessons.Where(x => x.CourseId == courseId).ToList();

            // Başlıkta göstermek için Kurs adını bul
            var course = _context.Courses.Find(courseId);
            ViewBag.CourseName = course?.Title;
            ViewBag.CourseId = courseId; // Yeni ders eklerken lazım olacak

            return View(lessons);
        }

        // --- DERS EKLEME ---
        [HttpGet]
        public IActionResult Create(int courseId)
        {
            var course = _context.Courses.Find(courseId);
            if (course == null) return NotFound();

            ViewBag.CourseName = course.Title;
            var lesson = new Lesson { CourseId = courseId };
            return View(lesson);
        }

        [HttpPost]
        public IActionResult Create(Lesson lesson)
        {
            // Course nesnesi formdan gelmediği için validasyonu kaldır
            ModelState.Remove("Course");

            if (ModelState.IsValid)
            {
                _context.Lessons.Add(lesson);
                _context.SaveChanges();

                // Düzeltme: Kaydettikten sonra o kursun DERS listesine dön
                return RedirectToAction("List", new { courseId = lesson.CourseId });
            }

            // Hata varsa viewbagleri tekrar doldur
            var course = _context.Courses.Find(lesson.CourseId);
            ViewBag.CourseName = course?.Title;
            return View(lesson);
        }

        // --- DERS DÜZENLEME (EDIT) ---
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var lesson = _context.Lessons.Find(id);
            if (lesson == null) return NotFound();
            return View(lesson);
        }

        [HttpPost]
        public IActionResult Edit(Lesson lesson)
        {
            ModelState.Remove("Course");

            if (ModelState.IsValid)
            {
                _context.Lessons.Update(lesson);
                _context.SaveChanges();

                // Düzeltme: Düzenleme bitince o kursun DERS listesine dön
                return RedirectToAction("List", new { courseId = lesson.CourseId });
            }
            return View(lesson);
        }

        // --- DERS SİLME ---
        [HttpPost]
        public IActionResult DeleteAjax(int id)
        {
            var lesson = _context.Lessons.Find(id);
            if (lesson != null)
            {
                _context.Lessons.Remove(lesson);
                _context.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}