using Microsoft.AspNetCore.Http;
using Application.Exceptions;
using System.Threading.Tasks;

namespace API.Middlewares
{
    public interface IExceptionMiddleware
    {
        Task InvokeAsync(HttpContext context, RequestDelegate next);
    }

    public class ExceptionMiddleware :IMiddleware, IExceptionMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                // Przechodzi do kolejnego middleware
                await next.Invoke(context);
            }
            catch (NotFoundException ex)
            {
                // Obsługuje wyjątek NotFoundException
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync($"{{\"error\": \"{ex.Message}\"}}");
            }
            catch (BadCredentialsException ex)
            {
                // Obsługuje wyjątek BadCredentialsException
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync($"{{\"error\": \"{ex.Message}\"}}");
            }
            catch (Exception ex)
            {
                // Obsługuje wszystkie inne wyjątki
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync("{\"error\": \"Something went wrong\"}" + ex);
               
            }
        }
    }
}
