using GeorgeShop.DAL.Data;
using GeorgeShop.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeorgeShop.DAL.Repository
{
    public class CartRepository : GenericRepository<Cart> ,ICartRepository 
    {
        public CartRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
