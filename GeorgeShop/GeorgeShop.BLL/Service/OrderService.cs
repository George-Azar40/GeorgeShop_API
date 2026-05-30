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
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        Task<OrderResponse> IOrderService.GetOrder(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<OrderResponse>> GetUserOrders(string userId)
        {
            var orders = await _orderRepository.GetAllAsync(
              filter: u => u.UserId == userId,
              includes: new[]
              {
                   nameof(Order.OrderItems),
                   $"{nameof(Order.OrderItems)}.{nameof(OrderItem.Product)}",
                   $"{nameof(Order.OrderItems)}.{nameof(OrderItem.Product)}" + $".{nameof(Product.Translations)}"
              }
              );
            return orders.Adapt<List<OrderResponse>>();
        }
    }
}
