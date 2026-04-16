using GeorgeShop.DAL.DTO.Request;
using GeorgeShop.DAL.DTO.Response;
using GeorgeShop.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeorgeShop.BLL.Service
{
    public interface ICartService
    {
        Task<bool> AddToCart(AddToCartRequest request , String UserId, CancellationToken cancellation);
        Task<List<CartResponse>> GetCart(string UserId);
        Task<bool> UpdateQuantity(int productId, int count, string userId);
        Task<bool> RemoveItem(int productId , string userId);
        Task<bool> ClearCart(string userId);
    }
}
