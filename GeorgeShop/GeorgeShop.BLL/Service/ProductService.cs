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
            if(request.SubImages != null)
            {
                foreach(var image in request.SubImages)
                {
                    var imagePath = await _fileService.UploadAsync(image);
                    product.Images.Add(new ProductImage
                    {
                        ImagePath = imagePath
                    });
                }
            }

            await Task.Delay(5000, cancellationToken);
            await _productRepository.CreateAsync(product , cancellationToken);
        }


        public async Task<List<ProductResponse>> GetAllProductsAsync()
        {
            var product = await _productRepository.GetAllAsync(
                p => p.Status == EntityStatus.Active
                , new string[]
            {
                nameof(Product.Translations),
                nameof(Product.CreatedBy),
                nameof(Product.Images),

                //ProductService -> GetAllProductAsync
                nameof(Product.Brand)
            });
            return product.Adapt<List<ProductResponse>>();
        }

        public async Task<ProductResponse?> GetProduct(Expression<Func<Product, bool>> filter)
        {
            var product = await _productRepository.GetOne(filter, new string[]
            {
                nameof(Product.Translations),
                nameof(Product.CreatedBy),
                nameof(Product.Brand)
            });

            if(product == null)
            {
                return null;
            }
            
            return product.Adapt<ProductResponse>();
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _productRepository.GetOne(
                p=>p.Id == id ,
                includes: new string[] { nameof(Product.Images) }
                );
            if(product == null) return false;
            _fileService.Delete(product.MainImage);
            
            foreach(var image in product.Images)
            {
                _fileService.Delete(image.ImagePath);
            }

            return await _productRepository.DeleteAsync(product);
        }


        public async Task<bool> UpdateProduct(int id, ProductUpdateRequest request)
        {
            var product = await _productRepository.GetOne(p => p.Id == id, new string[]
            {
                nameof(Product.Translations),
                nameof(Product.Images),
            });

            if(product == null) return false;

            request.Adapt(product);

            var oldImage = product.MainImage;
            if(request.MainImage != null)
            {
                _fileService.Delete(oldImage);
                product.MainImage = await _fileService.UploadAsync(request.MainImage);
            }
            else
            {
                product.MainImage = oldImage;
            }
            
            if(request.SubImages != null)
            {
                foreach(var image in product.Images)
                {
                    _fileService.Delete(image.ImagePath);
                }
                product.Images.Clear();
                foreach(var image in request.SubImages)
                {
                    var imagePath = await _fileService.UploadAsync(image);
                    product.Images.Add(new ProductImage
                    {
                        ImagePath = imagePath
                    });
                }
            }

            if(request.newImages != null)
            {
                foreach (var image in request.newImages)
                {
                    var imagePath = await _fileService.UploadAsync(image);
                    product.Images.Add(new ProductImage
                    {
                        ImagePath = imagePath
                    });
                }
            }

            return await _productRepository.UpdateAsync(product);
            
        }

        public async Task<bool> ToogleStatus(int id)
        {
            var product = await _productRepository.GetOne(p=>p.Id==id);
            if(product == null) return false;

            product.Status = product.Status == EntityStatus.Active ?
                EntityStatus.Inactive : EntityStatus.Active;

            return await _productRepository.UpdateAsync(product);

        }


    }
}
