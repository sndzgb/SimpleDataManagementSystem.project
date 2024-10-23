using System.Net.Mime;
using System.Net;
using System.Text.Json;
using SimpleDataManagementSystem.Frontend.Web.Razor.Exceptions;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly RequestDelegate _next;


        public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }


        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var type = ex.GetType();

            if (type == typeof(HttpRequestException))
            {
                if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
                {
                    context.Items.Add("Error", "Invalid login attempt.");
                    context.Response.Redirect("/Unauthorized", true);
                    return;
                }

                //context.Items.Add("Error", "Invalid login attempt.");
                //context.Response.Redirect("/Error", true);
                //return;
            }
            
            


            if (type == typeof(WebApiCallException))
            {
                context.Items.Add("error", ex.Message);
            }

            if (type == typeof(Exception))
            {
                context.Items.Add("error", "An error occured while processing your request.");
            }

            await Task.CompletedTask;
        }
    }
}
