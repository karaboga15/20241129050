using internetprogramciligi1.Data;
using internetprogramciligi1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace internetprogramciligi1.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        public ProfileController(UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // PROFİL SAYFASI
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login", "Account");

            // Kullanıcının kayıtlarını (Enrollment) çek
            var myEnrollments = _context.Enrollments
                                    .Include(e => e.Course)
                                    .Where(e => e.UserId == user.Id)
                                    .ToList();

            ViewBag.UserEmail = user.Email;
            ViewBag.UserName = user.UserName;

            return View(myEnrollments); // Listeyi View'a gönderiyoruz
        }

        // KURS BİTİRME İŞLEMİ (YENİ)
        [HttpPost]
        public async Task<IActionResult> CompleteCourse(int courseId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Json(new { success = false, message = "Oturum bulunamadı." });

            var enrollment = _context.Enrollments
                                     .FirstOrDefault(e => e.UserId == user.Id && e.CourseId == courseId);

            if (enrollment == null)
            {
                return Json(new { success = false, message = "Bu kursa kayıtlı değilsiniz." });
            }

            enrollment.IsCompleted = true; // Tamamlandı işaretle
            _context.SaveChanges();

            var courseName = _context.Courses.Find(courseId)?.Title;

            return Json(new { success = true, message = $"Tebrikler! '{courseName}' isimli kursu başarıyla bitirdiniz." });
        }

        // KURSA KAYDOLMA
        [HttpPost]
        public async Task<IActionResult> Enroll(int courseId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Json(new { success = false }); // Login'e yönlendirme JS tarafında

            bool alreadyEnrolled = _context.Enrollments.Any(e => e.UserId == user.Id && e.CourseId == courseId);
            if (alreadyEnrolled)
            {
                return Json(new { success = false, message = "Zaten bu kursa kayıtlısınız." });
            }

            var enrollment = new Enrollment
            {
                UserId = user.Id,
                CourseId = courseId,
                IsCompleted = false
            };

            _context.Enrollments.Add(enrollment);
            _context.SaveChanges();

            return Json(new { success = true, message = "Kursa başarıyla kaydoldunuz!" });
        }

        // ŞİFRE DEĞİŞTİRME
        [HttpPost]
        public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Json(new { success = false, message = "Oturum hatası." });

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            if (result.Succeeded)
                return Json(new { success = true, message = "Şifreniz güncellendi." });
            else
                return Json(new { success = false, message = "Şifre değiştirilemedi." });
        }
    }
}