using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    // the repository serves to avoid accessing the database direcction with ApplicationDbContext _db passed to
    // every controller
    // a generic repository of type T, where T is a class
    public interface IRepository<T> where T : class
    {
        // T can be any class
        // we want an expression, inside the expression there is a function, that the first
        // parameter is the generic class T, and the output is boolean and we will call that filter
        T GetFirstOrDefault(Expression<Func<T,bool>> filter, string? includeProperties = null);
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter=null, string? includeProperties = null);
        void Add(T entity);
        void Remove(T entity);
        // we remove more than one object, so a list of objects
        void RemoveRange(IEnumerable<T> entity);

    }
}
