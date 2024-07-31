using Microsoft.AspNetCore.Diagnostics;
using PokeApp.Core.Application.Responses;

namespace PokeApp.Presentation.API.Middlewares
{

    public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            string title = "";
            title = exception.Message;

            var data = new
            {
                Instance = httpContext.Request.Path,
                Title = title,
                Status = httpContext.Response.StatusCode
            };

            logger.LogError($"{DateTime.Now.ToString("yyyy-MM-dd")} => {exception.Message} || {exception.StackTrace}");

            await httpContext.Response.WriteAsJsonAsync(Result<object>.Fail(data, exception.Message), cancellationToken).ConfigureAwait(false);
            return true;
        }
    }
}
