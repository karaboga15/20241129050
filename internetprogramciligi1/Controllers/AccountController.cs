using internetprogramciligi1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace internetprogramciligi1.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        // --- LOGIN (GİRİŞ) İŞLEMLERİ ---
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Username); // Username yerine Email ile giriş
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                ModelState.AddModelError("", "Email veya şifre hatalı.");
            }
            return View(model);
        }

        // --- REGISTER (KAYIT OL) İŞLEMLERİ ---
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // ÖNEMLİ: Kayıt olan kullanıcıya otomatik "Uye" rolü veriyoruz.
                    // Rol veritabanında yoksa önce oluşturur, hata vermesin diye.
                    if (!await _roleManager.RoleExistsAsync("Uye"))
                    {
                        await _roleManager.CreateAsync(new IdentityRole("Uye"));
                    }

                    await _userManager.AddToRoleAsync(user, "Uye");

                    // Kayıt bitti, otomatik giriş yap ve ana sayfaya gönder
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        // --- LOGOUT (ÇIKIŞ) ---
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Json(new { success = true });
        }
    }
}