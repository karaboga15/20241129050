using internetprogramciligi1.Data;
using internetprogramciligi1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace internetprogramciligi1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context; // EKLENDİ

        // Constructor'a context parametresi eklendi
        public UsersController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context; // ATANDI
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var userList = new List<UserViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userList.Add(new UserViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = roles
                });
            }
            return View(userList);
        }

        [HttpPost]
        public async Task<IActionResult> AssignAdmin(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return Json(new { success = false, message = "Kullanıcı bulunamadı." });

            if (!await _roleManager.RoleExistsAsync("Admin"))
                await _roleManager.CreateAsync(new IdentityRole("Admin"));

            if (!await _userManager.IsInRoleAsync(user, "Admin"))
            {
                await _userManager.AddToRoleAsync(user, "Admin");
            }
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> RevokeAdmin(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return Json(new { success = false, message = "Kullanıcı bulunamadı." });

            if (User.Identity.Name == user.UserName)
            {
                return Json(new { success = false, message = "Kendi yetkinizi alamazsınız!" });
            }

            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                await _userManager.RemoveFromRoleAsync(user, "Admin");
            }
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAjax(string id)
        {
            if (string.IsNullOrEmpty(id)) return Json(new { success = false, message = "ID boş geldi!" });

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return Json(new { success = false, message = "Kullanıcı bulunamadı." });

            if (User.Identity?.Name == user.UserName)
            {
                return Json(new { success = false, message = "Kendinizi silemezsiniz!" });
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
                return Json(new { success = true, message = "Silindi" });
            else
                return Json(new { success = false, message = "Hata oluştu." });
        }

        // KULLANICININ ALDIĞI KURSLARI GÖSTER (Admin)
        public IActionResult UserCourses(string userId)
        {
            var enrollments = _context.Enrollments
                                      .Include(e => e.Course)
                                      .Include(e => e.User)
                                      .Where(e => e.UserId == userId)
                                      .ToList();

            var user = _context.Users.Find(userId);
            ViewBag.UserName = user?.UserName;

            return View(enrollments);
        }
    }
}