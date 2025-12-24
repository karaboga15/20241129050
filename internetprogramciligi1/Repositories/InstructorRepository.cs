using internetprogramciligi1.Data;
using internetprogramciligi1.Models;

namespace internetprogramciligi1.Repositories
{
    public class InstructorRepository
    {
        private readonly ApplicationDbContext _context;

        public InstructorRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Instructor> GetAll()
        {
            return _context.Instructors.ToList();
        }

        public Instructor GetById(int id)
        {
            return _context.Instructors.Find(id);
        }

        public void Add(Instructor instructor)
        {
            _context.Instructors.Add(instructor);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var instructor = _context.Instructors.Find(id);
            if (instructor != null)
            {
                _context.Instructors.Remove(instructor);
                _context.SaveChanges();
            }
        }
        public void Update(Instructor instructor)
        {
            _context.Instructors.Update(instructor);
            _context.SaveChanges();
        }
    }
}