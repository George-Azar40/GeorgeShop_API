
using GeorgeShop.BLL.Mapping;
using GeorgeShop.BLL.Service;
using GeorgeShop.DAL.Data;
using GeorgeShop.DAL.Models;
using GeorgeShop.DAL.Repository;
using GeorgeShop.DAL.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace GeorgeShop.PL
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddLocalization(options =>options.ResourcesPath = "");

            const string defaultCulture = "en";
            var supportedCultures = new[]
            {
                new CultureInfo(defaultCulture),
                new CultureInfo("ar"),
            };
                builder.Services.Configure<RequestLocalizationOptions>(options => {
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
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                    policy =>
                    {
                        policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                    });
            });

            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<ICategoryService,CategoryService>();
            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
            builder.Services.AddScoped<ISeedData,RoleSeedData>();

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;

                options.Password.RequireDigit = true; // 0-9
                options.Password.RequireLowercase = true; // a-z
                options.Password.RequireUppercase = true; // A-Z
                options.Password.RequireNonAlphanumeric = true; // ! @ $ # * %
                options.Password.RequiredLength = 10; // minimun pass length => 10 letter

                options.Lockout.MaxFailedAccessAttempts = 5; // lock user if failed 5 times
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // block for 5 minutes


            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddTransient<IEmailSender, EmailSender>();


            builder.Services.AddScoped<IFileService, FileService>();
            builder.Services.AddScoped<IProductService ,  ProductService>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();

            //define JWT
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })

                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = builder.Configuration["Jwt:Issuer"],
                            ValidAudience = builder.Configuration["Jwt:Audience"],
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
                        };
                    });

            MapsterConfig.MapsterConfigRegister();

            var app = builder.Build();


          

            app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);



            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }



            app.UseCors(MyAllowSpecificOrigins);

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseAuthorization();


            app.MapControllers();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var seeders = services.GetServices<ISeedData>();
                foreach(var seeder in seeders)
                {
                    await seeder.DataSeed();
                }
            }

            try
            {
                app.Run();
            }
            catch (Exception ex)
            {
                File.WriteAllText("error.txt", ex.ToString());
                throw;
            }
        }
    }
}
