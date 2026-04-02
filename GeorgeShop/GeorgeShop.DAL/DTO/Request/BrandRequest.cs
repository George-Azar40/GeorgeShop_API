using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeorgeShop.DAL.DTO.Request
{
    public class BrandRequest
    {
        public string Name { get; set; }

        //BrandRequest
        public IFormFile BrandImage { get; set; }


    }
}
