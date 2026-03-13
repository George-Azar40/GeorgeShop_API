using GeorgeShop.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GeorgeShop.DAL.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync(string[] strings);
        Task<T> CreateAsync(T category , CancellationToken cancellationToken);
        Task<T?> GetOne(Expression<Func<T, bool>> filter, string[]? includes = null);
        Task<bool> DeleteAsync(T entity);
        

        Task<bool> UpdateAsync(T entity);

    }
}
