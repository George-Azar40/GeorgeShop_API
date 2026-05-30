using GeorgeShop.BLL.Service;
using GeorgeShop.DAL.DTO.Request;
using GeorgeShop.DAL.Models;
using GeorgeShop.PL.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Security.Claims;

namespace GeorgeShop.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {

        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IOrderService _orderService;
        public OrdersController(IOrderService orderService, IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
            _orderService = orderService; 
        }


        [HttpGet("")]
        public async Task<IActionResult> GetMyOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var orders = await _orderService.GetUserOrders(userId);
            return Ok(new
            {
                data = orders,
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserOrder(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var order = await _orderService.GetUserOrder(userId,id);
            return Ok(new
            {
                data = order ,
            });
        }

        [HttpGet("admin")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllOrders([FromQuery] OrderStatusEnum status = OrderStatusEnum.Pending)
        {
            var orders = await _orderService.GetAllOrders(status);
            return Ok(new
            {
                data = orders,
            });
        }
        [HttpPatch("admin/{id}/status")]
        public async Task<IActionResult> ChangeStatus(int id, [FromBody] ChangeOrderStatusRequest status)
        {
            var result = await _orderService.ChangeOrderStatus(id, status);
            if(!result) return BadRequest();

            return Ok();
        }

    }
}
