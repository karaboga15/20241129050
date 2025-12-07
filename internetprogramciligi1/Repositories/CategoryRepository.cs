using internetprogramciligi1.Data;
using internetprogramciligi1.Models;

namespace internetprogramciligi1.Repositories
{
    public class CategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. Hepsini Getir
        public List<Category> GetAll()
        {
            return _context.Categories.ToList();
        }

        // 2. ID'ye Göre Getir (Tek Bir Tane)
        public Category GetById(int id)
        {
            return _context.Categories.Find(id);
        }

        // 3. Ekle
        public void Add(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
        }

        // 4. Sil
        public void Delete(int id)
        {
            var category = _context.Categories.Find(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
            }
        }
    }
}