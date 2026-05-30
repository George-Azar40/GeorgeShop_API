using GeorgeShop.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeorgeShop.DAL.DTO.Request
{
    public class ChangeOrderStatusRequest
    {
        public OrderStatusEnum Status{ get; set; }
    }
}
