using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace internetprogramciligi1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        
        public IActionResult Index()
        {
            return View(); 
        }

        
        public IActionResult Categories()
        {
           
            return RedirectToAction("Index", "Category");
        }

       
        public IActionResult Courses()
        {
            return RedirectToAction("Index", "Course");
        }
    }
}