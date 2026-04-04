using GeorgeShop.DAL.Data;
using GeorgeShop.DAL.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace GeorgeShop.PL.Extensions
{
    public static class InfrastructureExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
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


            services.AddAuthentication(options =>
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
                        ValidIssuer = config["Jwt:Issuer"],
                        ValidAudience = config["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:SecretKey"]))
                    };
                });


            return services;

        }
    }
}
