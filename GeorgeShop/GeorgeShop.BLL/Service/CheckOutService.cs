using GeorgeShop.DAL.DTO.Request;
using GeorgeShop.DAL.DTO.Response;
using GeorgeShop.DAL.Models;
using GeorgeShop.DAL.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeorgeShop.BLL.Service
{
    public class CheckOutService : ICheckOutService
    {
        private readonly ICartRepository _cartRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOrderRepository _orderRepository;
        public CheckOutService(ICartRepository cartRepository ,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor,
            IOrderRepository orderRepository
            ) 
        {
            _cartRepository = cartRepository;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _orderRepository = orderRepository;
        }
        public async Task<CheckoutResponse> ProcessCheckout(string userId, CheckoutRequest request,CancellationToken cancellation)
        {
            var cartItems = await _cartRepository.GetAllAsync(
                filter: c => c.UserId == userId,
                includes: new []{nameof(Cart.Product) ,
                $"{ nameof(Cart.Product) }.{nameof(Product.Translations)}"
                }

                );

            if (!cartItems.Any())
            {
                return new CheckoutResponse
                {
                    Success = false,
                    Error = "cart is empty"
                };
            }

            var user = await _userManager.FindByIdAsync(userId);
            var city = request.City ?? user.City;
            // but user may not add city never

            if (city is null)
            {
                return new CheckoutResponse
                {
                    Success = false,
                    Error = "city is required"
                };
            }

            var street = request.Street ?? user.Street;
            // but user may not add street never

            if (street is null)
            {
                return new CheckoutResponse
                {
                    Success = false,
                    Error = "street is required"
                };
            }

            var phoneNumber = request.PhoneNumber ?? user.PhoneNumber;
            // but user may not add phoneNumber never

            if (phoneNumber is null)
            {
                return new CheckoutResponse
                {
                    Success = false,
                    Error = "phone is required"
                };
            }

            foreach (var item in cartItems)
            {
                if (item.Count > item.Product.Quantity)
                {
                    return new CheckoutResponse
                    {
                        Success = false,
                        Error = "doesn't have enough stock"
                    };
                }
            }


            var order = new Order()
            {
                UserId = userId,
                City = city,
                Street = street,
                PhoneNumber = phoneNumber,
                PaymentMethod = request.PaymentMethod,
                AmountPaid = cartItems.Sum(x => x.Product.Price * x.Count),
                OrderItems = cartItems.Select(s=>new OrderItem
                {
                    ProductId = s.ProductId,
                    Quantity = s.Count,
                    UnitPrice = s.Product.Price,
                    TotalPrice = s.Product.Price * s.Count,

                }).ToList()
            };

            await _orderRepository.CreateAsync(order, cancellation);



            if (request.PaymentMethod == PaymentMethod.Cash)
            {
                return new CheckoutResponse
                {
                    Success = true,
                    Error = " "
                };
            }

            if (request.PaymentMethod == PaymentMethod.Visa)
            {
                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string> { "card" },
                    LineItems = new List<SessionLineItemOptions>(),
                    Mode = "payment",
                    SuccessUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/checkout/success",
                    CancelUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/checkout/cancel",

                };


                foreach(var item in cartItems)
                {
                    options.LineItems.Add(
                        new SessionLineItemOptions
                        {
                            PriceData = new SessionLineItemPriceDataOptions
                            {
                                Currency = "USD",
                                ProductData = new SessionLineItemPriceDataProductDataOptions
                                {
                                    Name = item.Product.Translations.FirstOrDefault(t => t.Language == "en").Name
                                },
                                UnitAmount = (long)(item.Product.Price * 100) ,
                                //to convert from Cent to Dollar
                            },
                            Quantity = item.Count,
                        });
                }

                var service = new SessionService();
                var session = service.Create(options);


                return new CheckoutResponse
                {
                    Success = true,
                    StripeUrl = session.Url
                };


            }
            return new CheckoutResponse
            {
                Success = false,
                Error = "Invalid payment method"
            };
        }
    }
}
