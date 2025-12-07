using internetprogramciligi1.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore; // Bu kütüphane şart
using Microsoft.EntityFrameworkCore;

namespace internetprogramciligi1.Data
{
    // DİKKAT: Artık 'DbContext' değil 'IdentityDbContext' sınıfından miras alıyor
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
    }
}