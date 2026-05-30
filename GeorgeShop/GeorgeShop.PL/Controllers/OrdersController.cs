using GeorgeShop.BLL.Service;
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

    }
}
