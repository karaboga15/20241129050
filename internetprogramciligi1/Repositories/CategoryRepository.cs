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

        
        public List<Category> GetAll()
        {
            return _context.Categories.ToList();
        }

        
        public Category GetById(int id)
        {
            return _context.Categories.Find(id);
        }

       
        public void Add(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
        }

        
        public void Delete(int id)
        {
            var category = _context.Categories.Find(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
            }
        }
        public void Update(Category category)
        {
            _context.Categories.Update(category);
            _context.SaveChanges();
        }
    }
}