using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeorgeShop.DAL.DTO.Response
{
    public class CheckoutResponse
    {
        public int OrderId  { get; set; }
        public string? StripeUrl { get; set; } //User can Pay Cash
        public bool Success { get; set; }
        public string? Error { get; set; }

    }
}
