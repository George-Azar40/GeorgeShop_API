using Microsoft.Extensions.Options;

namespace GeorgeShop.PL.Extensions
{
    public static class AppExtensions
    {
        public static WebApplication UseApplicationPipeline(this WebApplication app)
        {
            app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);
            
            
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
            app.UseCors(MyAllowSpecificOrigins);

            app.UseHttpsRedirection();
            

            app.UseStaticFiles();

            app.UseAuthorization();


            app.MapControllers();

            return app;
        }
    }
}
