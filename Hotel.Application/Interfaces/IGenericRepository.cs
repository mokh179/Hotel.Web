using Hotel.Entities.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Application.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        IQueryable<T> Entities { get; }  
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);

        Task AddRangeAsync(IEnumerable<T> entities);
        Task ExecuteUpdateAsync(Expression<Func<T, bool>> predicate, Action<SetPropertyCalls<T>> setProperties);
        Task ExecuteSoftDeleteAsync(Expression<Func<T, bool>> predicate);
    }
}
