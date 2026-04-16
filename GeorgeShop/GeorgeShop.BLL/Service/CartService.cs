using GeorgeShop.DAL.DTO.Request;
using GeorgeShop.DAL.DTO.Response;
using GeorgeShop.DAL.Models;
using GeorgeShop.DAL.Repository;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeorgeShop.BLL.Service
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;

        public CartService(ICartRepository cartRepository, IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }
        public async Task<bool> AddToCart(AddToCartRequest request, string UserId , CancellationToken cancellation)
        {

            var product = await _productRepository.GetOne(p => p.Id == request.ProductId);
            if (product == null) return false;
            var ExistingItem = await _cartRepository.GetOne(
                c=>c.ProductId == request.ProductId && c.UserId == UserId
                );


            var currentCount = ExistingItem?.Count  ?? 0;
            //if ExistingItem is null let it zero
            
            var newCount = currentCount + request.Count;
            if(newCount > product.Quantity) return false;


            if (ExistingItem != null )
            {
                ExistingItem.Count = newCount;
                await _cartRepository.UpdateAsync(ExistingItem);
            }
            else
            {
                var cartItem = request.Adapt<Cart>();
                cartItem.UserId = UserId;
                await _cartRepository.CreateAsync(cartItem , cancellation) ;
            }

            return true;
        }

        public Task<bool> ClearCart(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<CartResponse>> GetCart(string UserId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveItem(int productId, string userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateQuantity(int productId, int count, string userId)
        {
            throw new NotImplementedException();
        }
    }
}
