using GeorgeShop.DAL.DTO.Request;
using GeorgeShop.DAL.DTO.Response;
using GeorgeShop.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GeorgeShop.BLL.Service
{
    public interface IBrandService
    {
        Task<BrandResponse> CreateAsync(BrandRequest request);
        Task<List<BrandResponse>> GetAllBrands();
        Task<BrandResponse> GetBrand(Expression<Func<Brand, bool>> filter);
        Task<BrandResponse?> DeleteBrandAsync(int id);
    }
}
