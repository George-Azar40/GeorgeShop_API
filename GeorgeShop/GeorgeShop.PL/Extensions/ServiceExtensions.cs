using GeorgeShop.BLL.Service;
using GeorgeShop.DAL.Repository;
using GeorgeShop.DAL.Utilities;

namespace GeorgeShop.PL.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<ISeedData, RoleSeedData>();
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IBrandService, BrandService>();
            services.AddScoped<IBrandRepository, BrandRepository>();

            return services;
        }
    }
}
