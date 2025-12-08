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

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

       
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
                
                var user = await _userManager.FindByNameAsync(model.Username);

                if (user == null)
                {
                   
                    user = new IdentityUser { UserName = model.Username };

                    
                    var createResult = await _userManager.CreateAsync(user, model.Password);

                    if (createResult.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    
                    var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }

                
                ModelState.AddModelError("", "Giriş başarısız. Şifre yanlış olabilir.");
            }
            return View(model);
        }

        [HttpPost] 
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            
            return Json(new { success = true });
        }
    }
}