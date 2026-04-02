using GeorgeShop.DAL.DTO.Request;
using GeorgeShop.DAL.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeorgeShop.BLL.Service
{
    public interface IBrandService
    {
        Task<BrandResponse> CreateAsync(BrandRequest request);
    }
}
