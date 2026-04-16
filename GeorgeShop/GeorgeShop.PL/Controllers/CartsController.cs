using GeorgeShop.BLL.Service;
using GeorgeShop.DAL.DTO.Request;
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
    public class CartsController : ControllerBase
    {
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly ICartService _cartService;
        public CartsController(ICartService cartService, IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
            _cartService = cartService;
        }

        [HttpPost("")]
        [Authorize]
        public async Task<IActionResult> AddToCart(AddToCartRequest request , CancellationToken cancellation)
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //Claim Gets information From Authorize
            var result = await _cartService.AddToCart(request , UserId , cancellation);

            if (!result)
                return BadRequest();

            return Ok(new
            {
                message = _localizer["Success"].Value,
            });
        }
        [HttpGet("")]
        
        public async Task<IActionResult> GetCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var items = await _cartService.GetCart(userId);
            return Ok(new
            {
                data = items
            });
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> RemoveItem([FromRoute] int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var removed = await _cartService.RemoveItem(productId, userId);

            if(!removed) return  BadRequest();                   

            return Ok();
        }

        [HttpPatch("productId")]
        public async Task<IActionResult> UpdateQuantity([FromRoute] int productId , [FromBody] UpdateCartRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var updated = await _cartService.UpdateQuantity(productId,request.Count,userId);

            if(!updated) return BadRequest();

            return Ok();

        }


    }

}
