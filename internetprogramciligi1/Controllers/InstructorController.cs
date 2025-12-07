using internetprogramciligi1.Data;
using internetprogramciligi1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace internetprogramciligi1.Controllers
{
    [Authorize]
    public class InstructorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InstructorController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Eğitmenleri Listele
        public IActionResult Index()
        {
            var instructors = _context.Instructors.ToList();
            return View(instructors);
        }

        // Yeni Eğitmen Ekleme Sayfası
        public IActionResult Create()
        {
            return View();
        }

        // Eğitmeni Kaydet
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

        // Silme İşlemi (Hoca silinirse)
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
            // Eğer Repository kullanıyorsan:
            // _instructorRepo.Delete(id);

            // Eğer direkt Context kullanıyorsan (InstructorController'ı Repository'e çevirmediysek):
            var instructor = _context.Instructors.Find(id);
            if (instructor != null)
            {
                _context.Instructors.Remove(instructor);
                _context.SaveChanges();
            }

            return Json(new { success = true });
        }
    }
}