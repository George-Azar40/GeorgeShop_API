using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeorgeShop.DAL.DTO.Response
{
    public class ProductResponse
    {
        public int Id { get; set; }
        public string UserCreated {  get; set; }
        public string Name  { get; set; }
        public decimal Price { get; set; }
        public string CreatedBy { get; set; }
        //ProductResponse
        public string BrandName { get; set; }
        public string BrandImage { get; set; }
        public string MainImage { get; set; }
    }
}
