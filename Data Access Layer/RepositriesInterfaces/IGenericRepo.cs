using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.RepositriesInterfaces
{
    public interface IGenericRepo<T>
        where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        bool Add(T entity);
        bool Update(T entity);
        bool Delete(int id);
        bool SaveChanges();
        Task<bool> SaveChangesAsync();
        IEnumerable<T> GetFilter(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IQueryable<T>> include = null
        );
    }
}
