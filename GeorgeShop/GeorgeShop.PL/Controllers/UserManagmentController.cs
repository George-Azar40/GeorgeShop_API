using GeorgeShop.BLL.Service;
using GeorgeShop.DAL.DTO.Request;
using GeorgeShop.PL.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace GeorgeShop.PL.Controllers
{
    [Route("api/admin")]
    [ApiController]
    [Authorize]
    public class UserManagmentController : ControllerBase
    {
        private readonly IUserManagmentService _UserManagmentService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        public UserManagmentController(IUserManagmentService userManagmentService, IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
            _UserManagmentService = userManagmentService;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _UserManagmentService.GetAllUsers();
            return Ok(new
            {
                users
            });
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUser([FromRoute] string userId)
        {
            var user = await _UserManagmentService.GetUser(userId);
            return Ok(new
            {
                user
            });
        }
        [HttpPatch("{userId}/role")]
        public async Task<IActionResult> changeRole(string userId, [FromBody] changeRoleRequest request)
        {
            var result = await _UserManagmentService.ChangeRole(userId, request.newRole);
            if (!result) return BadRequest();

            return Ok();
        }

        [HttpPatch("{userId}/toogle-block")]
        public async Task<IActionResult> toogleBlock(string userId)
        {
            var result = await _UserManagmentService.ToogleBlockUser(userId);
            if (!result) return BadRequest();
            return Ok();
        }
    }
}
