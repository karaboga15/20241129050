using internetprogramciligi1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace internetprogramciligi1.Controllers
{
    [AllowAnonymous] // Herkes buraya girebilir
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // --- 1. SAYFAYI GÖSTER (GET) ---
        // Bu metod silindiği için 405 hatası alıyordun. Şimdi geri geldi.
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // --- 2. GİRİŞ YAP & OTOMATİK KAYIT (POST) ---
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Önce veritabanına bak: Bu kullanıcı var mı?
                var user = await _userManager.FindByNameAsync(model.Username);

                if (user == null)
                {
                    // --- YOKSA: OTOMATİK OLUŞTUR ---
                    user = new IdentityUser { UserName = model.Username };

                    // E-Posta zorunluluğunu kaldırdığımız için sadece şifreyle oluşturuyoruz
                    var createResult = await _userManager.CreateAsync(user, model.Password);

                    if (createResult.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    // --- VARSA: ŞİFRESİNİ KONTROL ET ---
                    var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }

                // Hata varsa
                ModelState.AddModelError("", "Giriş başarısız. Şifre yanlış olabilir.");
            }
            return View(model);
        }

        [HttpPost] // DİKKAT: Güvenlik için Logout işlemi POST olmalı
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            // Sayfa yönlendirmesi yapmıyoruz, JSON dönüyoruz. Javascript yönlendirecek.
            return Json(new { success = true });
        }
    }
}