
using GeorgeShop.BLL.Mapping;
using GeorgeShop.PL.Extensions;

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
            
            //Refactoring Long Program
            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddPresentation();

            MapsterConfig.MapsterConfigRegister();

            var app = builder.Build();

            //Refactoring Long Program
            app.UseApplicationPipeline();
            await app.SeedDataAsync();    

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
