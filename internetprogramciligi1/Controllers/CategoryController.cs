using internetprogramciligi1.Models;
using internetprogramciligi1.Repositories; // Repository klasörünü ekledik
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace internetprogramciligi1.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        // Artık DbContext yerine Repository kullanıyoruz
        private readonly CategoryRepository _categoryRepository;

        public CategoryController(CategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        // Listeleme
        public IActionResult Index()
        {
            var categories = _categoryRepository.GetAll(); // Repository'den çekiyoruz
            return View(categories);
        }

        // Ekleme Sayfası
        public IActionResult Create()
        {
            return View();
        }

        // Ekleme İşlemi
        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _categoryRepository.Add(category); // Repository'e "Ekle" diyoruz
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // Silme İşlemi
        public IActionResult Delete(int id)
        {
            _categoryRepository.Delete(id); // Repository'e "Sil" diyoruz
            return RedirectToAction("Index");
        }

        // --- AJAX İLE SİLME (Sayfa Yenilenmeden) ---
        [HttpPost]
        public IActionResult DeleteAjax(int id)
        {
            // Repository kullanarak sil
            _categoryRepository.Delete(id);

            // Geriye JSON formatında "Başarılı" mesajı dön
            return Json(new { success = true });
        }
    }
}