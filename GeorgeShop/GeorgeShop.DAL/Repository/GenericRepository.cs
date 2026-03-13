using GeorgeShop.DAL.Data;
using GeorgeShop.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GeorgeShop.DAL.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<T> CreateAsync(T entity, CancellationToken cancellationToken)
        {
            await _context.AddAsync(entity , cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<bool> DeleteAsync(T entity)
        {
            _context.Remove(entity);
            var affected = await _context.SaveChangesAsync();

            return affected > 0;
        }

        

        public async Task<List<T>> GetAllAsync(string[]? includes = null)
        {
            IQueryable<T> querry = _context.Set<T>();
            if(includes != null)
            {
                foreach(var include in includes)
                {
                    querry = querry.Include(include);
                }
            }
            return await querry.ToListAsync();
        }

        public async Task<T?> GetOne(Expression<Func<T,bool>> filter,string[]? includes = null)
        {
            IQueryable<T> querry = _context.Set<T>();
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    querry = querry.Include(include);
                }
            }

            return await querry.FirstOrDefaultAsync(filter);
        }


        public async Task<bool> UpdateAsync(T entity)
        {
            
            _context.Update(entity);
            var effected = await _context.SaveChangesAsync();

            return effected > 0;
        }


    }
}
