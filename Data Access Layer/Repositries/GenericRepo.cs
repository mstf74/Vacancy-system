using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Data_Access_Layer.Models;
using Data_Access_Layer.RepositriesInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Data_Access_Layer.Repositries
{
    public class GenericRepo<T> : IGenericRepo<T>
        where T : class
    {
        private readonly ApplicationContext _context;
        private readonly DbSet<T> _dbset;

        public GenericRepo(ApplicationContext context)
        {
            _context = context;
            _dbset = context.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return _dbset;
        }

        public T GetById(int id)
        {
            return _dbset.Find(id);
        }

        public bool Add(T entity)
        {
            _context.Add(entity);
            var result = SaveChanges();
            return result;
        }

        public bool Delete(int id)
        {
            var entity = _dbset.Find(id);
            _context.Remove(entity);
            var result = SaveChanges();
            return result;
        }

        public bool Update(T entity)
        {
            var entry = _context.Entry(entity);
            if (entry.State == EntityState.Detached)
                _context.Update(entry);
            else
                entry.State = EntityState.Modified;
            var result = SaveChanges();
            return result;
        }

        public IEnumerable<T> GetFilter(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IQueryable<T>> include = null
        )
        {
            IQueryable<T> query = _dbset;
            if (filter is not null)
                query = query.Where(filter);
            if (include is not null)
                query = include(query);
            return query.ToList();
        }

        public bool SaveChanges()
        {
            int result = _context.SaveChanges();
            return result > 0;
        }

        public async Task<bool> SaveChangesAsync()
        {
            int result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}
