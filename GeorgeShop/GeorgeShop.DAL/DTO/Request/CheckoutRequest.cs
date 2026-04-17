using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeorgeShop.DAL.DTO.Request
{
    public enum PaymentMethod
    {
        Cash = 1 ,
        Visa = 2 
    }
    public class CheckoutRequest
    {
        public PaymentMethod PaymentMethod {  get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public string? PhoneNumber { get; set; }    

    }
}
