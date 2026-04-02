using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeorgeShop.DAL.Models
{
    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string BrandImage { get; set;}
        public List<Product> Product { get; set; }
    }
}
