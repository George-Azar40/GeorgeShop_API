using Azure.Core;
using GeorgeShop.BLL.Service;
using GeorgeShop.DAL.DTO.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GeorgeShop.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        public AccountController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var result = await _authenticationService.RegisterAsync(request);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var result = await _authenticationService.LoginAsync(request);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("confirm")]
        public async Task<IActionResult> ConfirmEmail(string token , string id)
        {
            var isConfirmed =await _authenticationService.confirmEmailAsync(token , id);
            if(isConfirmed)
                return Ok();
            return BadRequest();
        }


        [HttpPost("SendCode")]
        public async Task<IActionResult> RequestPassword(ForgetPasswordRequest request)
        {
            var result = await _authenticationService.RequestPasswordResetAsync(request);

            if(!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpPost("resetPassword")]
        public async Task<IActionResult> PasswordReset(ResetPasswordRequest request)
        {
            var result = await _authenticationService.ResetPasswordAsync(request);

            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }
    }
}
