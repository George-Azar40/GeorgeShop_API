using GeorgeShop.DAL.DTO.Request;
using GeorgeShop.DAL.DTO.Response;
using GeorgeShop.DAL.Models;
using GeorgeShop.DAL.Repository;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GeorgeShop.BLL.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IFileService _fileService;
        public ProductService(IProductRepository productRepository , IFileService fileService)
        {
            _fileService = fileService;
            _productRepository = productRepository;
        }


        public async Task CreateProduct(ProductRequest request , CancellationToken cancellationToken)
        {
            var product = request.Adapt<Product>();
            if(request.MainImage != null)
            {
                var imagePath = await _fileService.UploadAsync(request.MainImage);
                product.MainImage = imagePath;
            }

            await Task.Delay(5000, cancellationToken);
            await _productRepository.CreateAsync(product , cancellationToken);
        }


        public async Task<List<ProductResponse>> GetAllProductsAsync()
        {
            var product = await _productRepository.GetAllAsync(new string[]
            {
                nameof(Product.Translations),
                nameof(Product.CreatedBy),
                nameof(Product.Brand)
            });
            return product.Adapt<List<ProductResponse>>();
        }

        public async Task<ProductResponse?> GetProduct(Expression<Func<Product, bool>> filter)
        {
            var product = await _productRepository.GetOne(filter, new string[]
            {
                nameof(Product.Translations),
                nameof(Product.CreatedBy)
            });

            if(product == null)
            {
                return null;
            }
            
            return product.Adapt<ProductResponse>();
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _productRepository.GetOne(p=>p.Id == id);
            if(product == null) return false;
            _fileService.Delete(product.MainImage);

            return await _productRepository.DeleteAsync(product);
        }

        
    }
}
