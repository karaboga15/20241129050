using internetprogramciligi1.Models;
using internetprogramciligi1.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using internetprogramciligi1.Hubs;
using internetprogramciligi1.Data; // Context için gerekli
using Microsoft.EntityFrameworkCore; // Include için gerekli
using System.Linq;

namespace internetprogramciligi1.Controllers
{
    public class CourseController : Controller
    {
        private readonly CourseRepository _courseRepo;
        private readonly CategoryRepository _categoryRepo;
        private readonly InstructorRepository _instructorRepo;
        private readonly IHubContext<GeneralHub> _hubContext;
        private readonly ApplicationDbContext _context; // EKLENDİ

        // Constructor'a context parametresi eklendi
        public CourseController(CourseRepository courseRepo, CategoryRepository categoryRepo, InstructorRepository instructorRepo, IHubContext<GeneralHub> hubContext, ApplicationDbContext context)
        {
            _courseRepo = courseRepo;
            _categoryRepo = categoryRepo;
            _instructorRepo = instructorRepo;
            _hubContext = hubContext;
            _context = context; // ATANDI
        }

        // 1. KULLANICI TARAFI (VİTRİN)
        [AllowAnonymous]
        public IActionResult Index(string search, int? categoryId)
        {
            var courses = _courseRepo.GetAll();

            if (!string.IsNullOrEmpty(search))
            {
                courses = courses.Where(x => x.Title.ToLower().Contains(search.ToLower())).ToList();
            }

            if (categoryId.HasValue)
            {
                courses = courses.Where(x => x.CategoryId == categoryId.Value).ToList();
            }

            ViewBag.Categories = _categoryRepo.GetAll();
            ViewBag.CurrentSearch = search;
            ViewBag.CurrentCategory = categoryId;

            return View(courses);
        }

        // 2. ADMIN TARAFI (YÖNETİM LİSTESİ)
        [Authorize(Roles = "Admin")]
        public IActionResult List(string search, int? categoryId)
        {
            var courses = _courseRepo.GetAll();

            if (!string.IsNullOrEmpty(search))
            {
                courses = courses.Where(x => x.Title.ToLower().Contains(search.ToLower())).ToList();
            }

            if (categoryId.HasValue)
            {
                courses = courses.Where(x => x.CategoryId == categoryId.Value).ToList();
            }

            ViewBag.Categories = _categoryRepo.GetAll();
            ViewBag.CurrentSearch = search;
            ViewBag.CurrentCategory = categoryId;

            return View(courses);
        }

        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            var course = _courseRepo.GetById(id);
            if (course == null) return NotFound();
            return View(course);
        }

        [AllowAnonymous]
        public IActionResult Watch(int id)
        {
            var course = _courseRepo.GetById(id);
            if (course == null) return NotFound();
            return View(course);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_categoryRepo.GetAll(), "Id", "Name");
            ViewBag.Instructors = new SelectList(_instructorRepo.GetAll(), "Id", "FullName");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(Course course)
        {
            if (ModelState.IsValid)
            {
                _courseRepo.Add(course);
                _hubContext.Clients.All.SendAsync("ReceiveInfo", "Yeni bir kurs eklendi: " + course.Title);
                return RedirectToAction("List");
            }

            ViewBag.Categories = new SelectList(_categoryRepo.GetAll(), "Id", "Name");
            ViewBag.Instructors = new SelectList(_instructorRepo.GetAll(), "Id", "FullName");
            return View(course);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var course = _courseRepo.GetById(id);
            if (course == null) return NotFound();

            ViewBag.Categories = new SelectList(_categoryRepo.GetAll(), "Id", "Name", course.CategoryId);
            ViewBag.Instructors = new SelectList(_instructorRepo.GetAll(), "Id", "FullName", course.InstructorId);
            return View(course);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Edit(Course course)
        {
            if (ModelState.IsValid)
            {
                _courseRepo.Update(course);
                return RedirectToAction("List");
            }

            ViewBag.Categories = new SelectList(_categoryRepo.GetAll(), "Id", "Name", course.CategoryId);
            ViewBag.Instructors = new SelectList(_instructorRepo.GetAll(), "Id", "FullName", course.InstructorId);
            return View(course);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteAjax(int id)
        {
            _courseRepo.Delete(id);
            return Json(new { success = true });
        }

        // KURSA KAYITLI ÜYELERİ GÖR (Admin)
        [Authorize(Roles = "Admin")]
        public IActionResult EnrolledUsers(int courseId)
        {
            var enrollments = _context.Enrollments
                                      .Include(e => e.User)
                                      .Where(e => e.CourseId == courseId)
                                      .ToList();
            return View(enrollments);
        }
    }
}