using GeorgeShop.DAL.DTO.Request;
using GeorgeShop.DAL.DTO.Response;
using GeorgeShop.DAL.Migrations;
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
    public class BrandService : IBrandService
    {

        private readonly IBrandRepository _brandRepository;
        private readonly IFileService _fileService;
        public BrandService(IBrandRepository brandRepository , IFileService fileService)
        {
            _brandRepository = brandRepository;
            _fileService = fileService;
        }

        public async Task<BrandResponse> CreateAsync(BrandRequest request)
        {
            var brand = request.Adapt<Brand>();

            if(request.BrandImage != null)
            {
                var imagePath = await _fileService.UploadAsync(request.BrandImage);
                brand.BrandImage = imagePath;
            }

            await _brandRepository.CreateBrandAsync(brand);
            return brand.Adapt<BrandResponse>();
        }




    }
}
