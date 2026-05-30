using GeorgeShop.DAL.DTO.Response;

namespace GeorgeShop.PL.Extensions.MiddleWare
{
    public class GlobalExeptionHandling
    {
        private readonly RequestDelegate _next;

        public GlobalExeptionHandling(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }catch
            (Exception ex)
            {
                var errorDetails = new ErrorDetails()
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "Service Error ... ",
                    InnerError = ex.InnerException.Message
                };
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(errorDetails);

            }

        }
    }
}
