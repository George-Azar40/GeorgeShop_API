using GeorgeShop.BLL.Service;
using GeorgeShop.DAL.DTO.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GeorgeShop.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CheckoutsController : ControllerBase
    {
        private readonly ICheckOutService _checkoutService;
        public CheckoutsController(ICheckOutService checkoutService)
        {
            _checkoutService = checkoutService;
        }

        [HttpPost("")]
        public async Task<IActionResult> Payment([FromBody]CheckoutRequest request,CancellationToken cancellation)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var response = await _checkoutService.ProcessCheckout(userId,request, cancellation);
            if (!response.Success)
            {
                return BadRequest(response.Error);
            }

            return Ok(response);
        }

        [HttpGet("success")]
        [AllowAnonymous]
        public async Task<IActionResult> Success([FromQuery] string sessionId)
        {
            var result = await _checkoutService.HandleSucess(sessionId);
            return Ok(new
            {
                message = "sucess"
            });
        }
    }
}
