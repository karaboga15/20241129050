using internetprogramciligi1.Models;
using internetprogramciligi1.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using internetprogramciligi1.Hubs;

namespace internetprogramciligi1.Controllers
{
    public class CourseController : Controller
    {
        private readonly CourseRepository _courseRepo;
        private readonly CategoryRepository _categoryRepo;
        private readonly InstructorRepository _instructorRepo;
        private readonly IHubContext<GeneralHub> _hubContext;

        public CourseController(CourseRepository courseRepo, CategoryRepository categoryRepo, InstructorRepository instructorRepo, IHubContext<GeneralHub> hubContext)
        {
            _courseRepo = courseRepo;
            _categoryRepo = categoryRepo;
            _instructorRepo = instructorRepo;
            _hubContext = hubContext;
        }

        // 1. KULLANICI TARAFI (VİTRİN) - Herkes Görebilir
        [AllowAnonymous]
        public IActionResult Index()
        {
            var courses = _courseRepo.GetAll();
            return View(courses);
        }

        // Kullanıcı Detay Sayfası
        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            var course = _courseRepo.GetById(id);
            if (course == null) return NotFound();
            return View(course);
        }

        // 2. ADMIN TARAFI (YÖNETİM LİSTESİ) - Sadece Admin
        [Authorize(Roles = "Admin")]
        public IActionResult List()
        {
            var courses = _courseRepo.GetAll();
            return View(courses); // Yeni oluşturacağımız Admin tablosuna gider
        }

        // Sadece Admin Kurs Ekleyebilir
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

                // Kaydettikten sonra Admin Listesine dön
                return RedirectToAction("List");
            }

            ViewBag.Categories = new SelectList(_categoryRepo.GetAll(), "Id", "Name");
            ViewBag.Instructors = new SelectList(_instructorRepo.GetAll(), "Id", "FullName");
            return View(course);
        }

        // Sadece Admin Silebilir
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteAjax(int id)
        {
            _courseRepo.Delete(id);
            return Json(new { success = true });
        }

        [AllowAnonymous] // Üye olmayan da izleyebilsin (veya [Authorize] yapabilirsin)
        public IActionResult Watch(int id)
        {
            var course = _courseRepo.GetById(id);
            if (course == null) return NotFound();
            return View(course);
        }
    }
}