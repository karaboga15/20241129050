using internetprogramciligi1.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace internetprogramciligi1.Repositories
{
    // T (Type): Herhangi bir veritabanı tablosu olabilir (Category, Product vb.)
    public class GenericRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        internal DbSet<T> dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            this.dbSet = _context.Set<T>();
        }

        // Tüm kayıtları getir
        public List<T> GetAll()
        {
            return dbSet.ToList();
        }

        
        public T GetById(int id)
        {
            return dbSet.Find(id);
        }

        
        public void Add(T entity)
        {
            dbSet.Add(entity);
            _context.SaveChanges();
        }

        
        public void Update(T entity)
        {
            dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            _context.Set<T>().Update(entity);
            _context.SaveChanges();
        }

        
        public void Delete(int id)
        {
            T entityToDelete = dbSet.Find(id);
            if (entityToDelete != null)
            {
                dbSet.Remove(entityToDelete);
                _context.SaveChanges();
            }
        }
    }
}