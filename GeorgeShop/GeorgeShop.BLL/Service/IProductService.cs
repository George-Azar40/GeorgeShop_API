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
    public interface IProductService
    {
        Task CreateProduct(ProductRequest request, CancellationToken cancellationToken);

        Task<List<ProductResponse>> GetAllProductsAsync();
        Task<ProductResponse?> GetProduct(Expression<Func<Product, bool>> filter);
        Task<bool> DeleteProductAsync(int id);
        Task<bool> UpdateProduct(int id, ProductUpdateRequest request);
    }
}
