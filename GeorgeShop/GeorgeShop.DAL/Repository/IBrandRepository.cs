using GeorgeShop.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeorgeShop.DAL.Repository
{
    public interface IBrandRepository : IGenericRepository<Brand>
    {
        Task<Brand> CreateBrandAsync(Brand entity);
    }
}
