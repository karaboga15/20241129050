using internetprogramciligi1.Models;
using internetprogramciligi1.Repositories; 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace internetprogramciligi1.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        
        private readonly CategoryRepository _categoryRepository;

        public CategoryController(CategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        
        public IActionResult Index()
        {
            var categories = _categoryRepository.GetAll();
            return View(categories);
        }

      
        public IActionResult Create()
        {
            return View();
        }

      
        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _categoryRepository.Add(category); 
                return RedirectToAction("Index");
            }
            return View(category);
        }

        
        public IActionResult Delete(int id)
        {
            _categoryRepository.Delete(id);
            return RedirectToAction("Index");
        }

       
        [HttpPost]
        public IActionResult DeleteAjax(int id)
        {
            
            _categoryRepository.Delete(id);

           
            return Json(new { success = true });
        }
        // CategoryController.cs içine ekle:

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var category = _categoryRepository.GetById(id);
            if (category == null) return NotFound();
            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _categoryRepository.Update(category); // Yeni eklediğimiz metod
                return RedirectToAction("Index");
            }
            return View(category);
        }
    }
}