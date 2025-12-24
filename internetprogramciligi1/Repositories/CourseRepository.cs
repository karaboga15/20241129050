using internetprogramciligi1.Data;
using internetprogramciligi1.Models;
using Microsoft.EntityFrameworkCore; 

namespace internetprogramciligi1.Repositories
{
    public class CourseRepository
    {
        private readonly ApplicationDbContext _context;

        public CourseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        
        public List<Course> GetAll()
        {
            return _context.Courses
                .Include(c => c.Category)
                .Include(c => c.Instructor)
                .ToList();
        }


        public Course GetById(int id)
        {
            return _context.Courses
                .Include(c => c.Category)
                .Include(c => c.Instructor)
                .Include(c => c.Lessons) // <--- BUNU EKLEMEK ŞART
                .FirstOrDefault(m => m.Id == id);
        }


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

        
        public int Count()
        {
            return _context.Courses.Count();
        }
    }
}