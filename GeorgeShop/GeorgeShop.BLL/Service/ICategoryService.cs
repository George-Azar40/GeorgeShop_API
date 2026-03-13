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
    public interface ICategoryService
    {
        Task<List<CategoryResponse>> GetAllCategories();
        Task<CategoryResponse> CreateCategory(CategoryRequest request , CancellationToken cancellationToken);

        Task<CategoryResponse?> GetCategory(Expression<Func<Category, bool>> filter);

        Task<bool> DeleteCategory(int  id);

        Task<CategoryResponse> UpdateCategory(int id);

    }
}
