using internetprogramciligi1.Models;
using internetprogramciligi1.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace internetprogramciligi1.Controllers
{
    [Authorize]
    public class CourseController : Controller
    {
       
        private readonly CourseRepository _courseRepo;
        private readonly CategoryRepository _categoryRepo;
        private readonly InstructorRepository _instructorRepo;

        public CourseController(CourseRepository courseRepo, CategoryRepository categoryRepo, InstructorRepository instructorRepo)
        {
            _courseRepo = courseRepo;
            _categoryRepo = categoryRepo;
            _instructorRepo = instructorRepo;
        }

       
        public IActionResult Index()
        {
            var courses = _courseRepo.GetAll();
            return View(courses);
        }

        
        public IActionResult Details(int id)
        {
            var course = _courseRepo.GetById(id);
            if (course == null) return NotFound();
            return View(course);
        }

        
        public IActionResult Create()
        {
            
            ViewBag.Categories = new SelectList(_categoryRepo.GetAll(), "Id", "Name");
            ViewBag.Instructors = new SelectList(_instructorRepo.GetAll(), "Id", "FullName");
            return View();
        }

       
        [HttpPost]
        public IActionResult Create(Course course)
        {
            if (ModelState.IsValid)
            {
                _courseRepo.Add(course);
                return RedirectToAction("Index");
            }

            ViewBag.Categories = new SelectList(_categoryRepo.GetAll(), "Id", "Name");
            ViewBag.Instructors = new SelectList(_instructorRepo.GetAll(), "Id", "FullName");
            return View(course);
        }

        
        public IActionResult Delete(int id)
        {
            _courseRepo.Delete(id);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult DeleteAjax(int id)
        {
            _courseRepo.Delete(id);
            return Json(new { success = true });
        }
    }
}