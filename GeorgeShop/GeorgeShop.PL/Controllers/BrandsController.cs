using GeorgeShop.BLL.Service;
using GeorgeShop.DAL.DTO.Request;
using GeorgeShop.PL.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace GeorgeShop.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {

        private readonly IBrandService _brandService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        public BrandsController(IBrandService brandService , IStringLocalizer<SharedResources> localizer)
        {
            _brandService = brandService;
            _localizer = localizer;
        }


        [HttpPost("")]
        public async Task<IActionResult> CreateBrandAsync([FromForm] BrandRequest request)
        {
            var response = await _brandService.CreateAsync(request);
            if(response == null)
            {
                return BadRequest();
            }

            return Ok(new
            {
                Message = "Brand Added Successfully",
                Success = true
            });
        }

        
    }
}
