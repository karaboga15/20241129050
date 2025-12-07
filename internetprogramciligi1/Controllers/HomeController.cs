using internetprogramciligi1.Data;
using internetprogramciligi1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace internetprogramciligi1.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context; // Veritabanı bağlantısı

        // Yapıcı metodda veritabanını çağırıyoruz
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // Parametre olarak 'searchString' (aranan kelime) alıyoruz
        public IActionResult Index(string searchString)
        {
            // 1. İstatistikleri Hesapla (Sayaçlar)
            ViewBag.CourseCount = _context.Courses.Count(); // Toplam Kurs Sayısı
            ViewBag.CategoryCount = _context.Categories.Count(); // Toplam Kategori Sayısı

            // -- Burası Eski Arama Kodumuz --
            var courses = _context.Courses.Include(c => c.Category).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                courses = courses.Where(x => x.Title.Contains(searchString) || x.Description.Contains(searchString));
            }

            return View(courses.ToList());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}