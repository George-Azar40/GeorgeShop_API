using GeorgeShop.BLL.Service;
using GeorgeShop.DAL.Repository;
using GeorgeShop.DAL.Utilities;
using Stripe;

namespace GeorgeShop.PL.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddScoped<ICategoryRepository, CategoryRepository>();

            services.AddScoped<ICategoryService, CategoryService>();
            
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            
            services.AddScoped<ISeedData, RoleSeedData>();
            
            services.AddTransient<IEmailSender, EmailSender>();
            
            services.AddScoped<IFileService, BLL.Service.FileService>();
            
            services.AddScoped<IProductService, BLL.Service.ProductService>();
            
            services.AddScoped<IProductRepository, ProductRepository>();
            
            services.AddScoped<IBrandService, BrandService>();
            
            services.AddScoped<IBrandRepository, BrandRepository>();
            
            services.AddScoped<ICartRepository, CartRepository>();
            
            services.AddScoped<ICartService , CartService>();

            services.Configure<StripeSettings>(Configuration.GetSection("Stripe"));
            StripeConfiguration.ApiKey = Configuration["Stripe:SecretKey"];


            return services;
        }
    }
}
