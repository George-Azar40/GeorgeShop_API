using GeorgeShop.DAL.DTO.Request;
using GeorgeShop.DAL.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeorgeShop.BLL.Service
{
    public interface ICheckOutService
    {
        Task<CheckoutResponse> ProcessCheckout(string userId,CheckoutRequest request,CancellationToken cancellation);
    }
}
