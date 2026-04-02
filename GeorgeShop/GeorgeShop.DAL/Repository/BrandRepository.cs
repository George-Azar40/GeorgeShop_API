using GeorgeShop.DAL.Data;
using GeorgeShop.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeorgeShop.DAL.Repository
{
    public class BrandRepository : GenericRepository<Brand> , IBrandRepository
    {

        private readonly ApplicationDbContext _context;
        public BrandRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Brand> CreateBrandAsync(Brand entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
