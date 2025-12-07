using internetprogramciligi1.Data;
using internetprogramciligi1.Models;
using Microsoft.EntityFrameworkCore; // Include için şart

namespace internetprogramciligi1.Repositories
{
    public class CourseRepository
    {
        private readonly ApplicationDbContext _context;

        public CourseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // İlişkileriyle beraber hepsini getir
        public List<Course> GetAll()
        {
            return _context.Courses
                .Include(c => c.Category)
                .Include(c => c.Instructor)
                .ToList();
        }

        // Tek bir tanesini detaylı getir
        public Course GetById(int id)
        {
            return _context.Courses
                .Include(c => c.Category)
                .Include(c => c.Instructor)
                .FirstOrDefault(m => m.Id == id);
        }

        // Filtreli Getir (Arama ve Vitrin için)
        public IQueryable<Course> GetQueryable()
        {
            return _context.Courses
               .Include(c => c.Category)
               .Include(c => c.Instructor);
        }

        public void Add(Course course)
        {
            _context.Courses.Add(course);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var course = _context.Courses.Find(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
                _context.SaveChanges();
            }
        }

        // İstatistikler için sayı ver
        public int Count()
        {
            return _context.Courses.Count();
        }
    }
}