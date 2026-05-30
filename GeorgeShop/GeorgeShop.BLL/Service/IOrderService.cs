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
    public interface IOrderService
    {
        Task<List<OrderResponse>> GetUserOrders(string userId);
        Task<OrderDetailsResponse?> GetUserOrder(string userId, int orderId);
        Task<bool> CancelOrder(string userId, int orderId);
        Task<List<OrderResponse>> GetAllOrders(OrderStatusEnum status);
        Task<bool> ChangeOrderStatus(int orderId, ChangeOrderStatusRequest request);
    }
}
