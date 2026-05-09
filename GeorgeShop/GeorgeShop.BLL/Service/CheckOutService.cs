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
        private readonly ICartService _cartService;
        private readonly IProductRepository _productRepository;
        private readonly IEmailSender _emailSender;
        public CheckOutService(ICartRepository cartRepository ,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor,
            IOrderRepository orderRepository,
            ICartService cartService,
            IProductRepository productRepository,
            IEmailSender emailSender
            ) 
        {
            _cartRepository = cartRepository;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _orderRepository = orderRepository;
            _cartService = cartService;
            _productRepository = productRepository;
            _emailSender = emailSender;
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
                    SuccessUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/api/checkouts/success?sessionId={{CHECKOUT_SESSION_ID}}",
                    CancelUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/api/checkouts/cancel",
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
                    order.StripeSessionId = session.Id;

                await _orderRepository.UpdateAsync(order);


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

        public async Task<CheckoutResponse> HandleSucess(string sessionId)
        {
            var order = await _orderRepository.GetOne(
                o => o.StripeSessionId == sessionId,
                includes: new[]
                {
                    nameof(Order.OrderItems),
                    $"{nameof(Order.OrderItems)}.{nameof(OrderItem.Product)}",
                    $"{nameof(Order.OrderItems)}.{nameof(OrderItem.Product)}.{nameof(Product.Translations)}",
                }
                );

            order.OrderStatus = OrderStatusEnum.Paid;
            await _orderRepository.UpdateAsync(order);

            await _cartService.ClearCart(order.UserId);

            var user = await _userManager.FindByIdAsync(order.UserId);
            await _emailSender.SendEmailAsync(user.Email, "order Confirmed", "<h2> Your order has been places successfully</h2>");

            var LowStockProducts = await _productRepository.DecreaseQuantityAsync(order.OrderItems);

            foreach (var item in LowStockProducts)
            {
                if (LowStockProducts != null)
                {
                    await _emailSender.SendEmailAsync("georgeazar456@gmail.com", "Low Stock Alert",
                            $"<h2>Current Quantity is : {item.Quantity} -- for this Product : " +
                            $"{item.Translations.FirstOrDefault(e => e.Language == "en").Name}</h2>");

                }
            }
            

            return new CheckoutResponse()
            {
                Success = true,
                OrderId = order.Id
            };
        }
    }
}
