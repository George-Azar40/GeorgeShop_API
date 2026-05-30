namespace GeorgeShop.PL.Extensions.MiddleWare
{
    public static class customMiddleWareExtenssions
    {
        public static IApplicationBuilder UseCustomMiddle(this IApplicationBuilder app)
        {
            return app.UseMiddleware<CustomMiddleWare>();
        }
    }
    public class CustomMiddleWare
    {
        private readonly RequestDelegate _next;

        public CustomMiddleWare(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            Console.WriteLine("Processing request");
            await _next(context);
            Console.WriteLine("Processing response");
        }
    }
}
