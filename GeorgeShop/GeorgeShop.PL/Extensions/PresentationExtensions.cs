using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace GeorgeShop.PL.Extensions
{
    public static class PresentationExtensions
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            services.AddOpenApi();
            services.AddLocalization(options => options.ResourcesPath = "");

            const string defaultCulture = "en";
            var supportedCultures = new[]
            {
                new CultureInfo(defaultCulture),
                new CultureInfo("ar"),
            };
            services.Configure<RequestLocalizationOptions>(options => {
                options.DefaultRequestCulture = new RequestCulture(defaultCulture);
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;

                //options.RequestCultureProviders.Clear();
                //    options.RequestCultureProviders.Add(new QueryStringRequestCultureProvider
                //    {
                //        QueryStringKey = "lang"
                //    });
                options.RequestCultureProviders.Add(new AcceptLanguageHeaderRequestCultureProvider());
            });



            //core Policy => to let Front end take access to api
            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                    policy =>
                    {
                        policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                    });
            });

            return services;


        }
    }
}
