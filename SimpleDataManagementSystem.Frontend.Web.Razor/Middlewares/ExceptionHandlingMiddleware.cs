using System.Net.Mime;
using System.Net;
using System.Text.Json;
using SimpleDataManagementSystem.Frontend.Web.Razor.Exceptions;
using System;
using Microsoft.AspNetCore.Mvc.Filters;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Read;

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

        // TODO add server error message with additional error descriptions (List<string>)
        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var type = ex.GetType();
            bool exceptionHandled = false;

            // TODO test
            // comment?
            if (type == typeof(HttpRequestException))
            {
                // if auth token expired, triggered before sending the request
                if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
                {
                    context.Items.Add("error", "Invalid login attempt.");
                    context.Response.Redirect("/Unauthorized", true);
                    exceptionHandled = true;
                    //return;
                }
                // ... or request timed out?

                //context.Items.Add("Error", "Invalid login attempt.");
                //context.Response.Redirect("/Error", true);
                //return;
            }
            
            //if (type == typeof(DivideByZeroException))
            //{
            //    // remove, then add
            //    context.Items.TryAdd("error", "Divide by zero attempted.");
            //    context.Response.StatusCode = 400;
            //    //context.Response.Redirect("/Unauthorized", true);
            //    return;
            //    //context.Items.Add("Error", "Invalid login attempt.");
            //    //context.Response.Redirect("/Error", true);
            //    //return;
            //}
            
            if (type == typeof(WebApiCallException))
            {
                // TODO return 400 & let Error.cshtml handle the rest

                // Page - PageModel
                //context.Request.RouteValues.Values

                var exception = ex as WebApiCallException;
                // TODO put on top, outside individual exception type checks
                if (context.Request.Headers["X-Requested-With"] == "XMLHttpRequest") // ajax request
                {
                    context.Response.StatusCode = exception?.Error?.StatusCode ?? StatusCodes.Status500InternalServerError;
                    context.Response.ContentType = "application/json";
                    //context.Response.StatusCode = 400;
                    
                    string json = JsonSerializer.Serialize(
                        new ViewModels.Read.ErrorViewModel(
                            exception?.Error?.StatusCode ?? StatusCodes.Status500InternalServerError, 
                            exception?.Error?.Message, 
                            exception?.Error?.Errors
                        )
                    );

                    await context.Response.WriteAsync(json);
                    await context.Response.CompleteAsync();
                }
                else
                {
                    context.Response.StatusCode = exception?.Error?.StatusCode ?? StatusCodes.Status500InternalServerError;
                    context.Items.TryAdd(
                        nameof(ErrorViewModel), 
                        new ViewModels.Read.ErrorViewModel(
                            exception?.Error?.StatusCode ?? StatusCodes.Status500InternalServerError,
                            exception?.Error?.Message ?? "Bad request occured. Please revise the data sent to the server.",
                            exception?.Error?.Errors
                        )
                    );
                }

                exceptionHandled = true;
            }

            if (!exceptionHandled)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Items.Add(
                    nameof(ErrorViewModel),
                    new ViewModels.Read.ErrorViewModel(
                        StatusCodes.Status500InternalServerError,
                        "Internal server error.",
                        null
                    )
                );
            }

            //if (type == typeof(Exception))
            //{
                // TODO log
                //context.Response.StatusCode = 400;
                //context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                //context.Items.Add(
                //    nameof(ErrorViewModel),
                //    new ViewModels.Read.ErrorViewModel(
                //        StatusCodes.Status500InternalServerError,
                //        "Internal server error.",
                //        null
                //    )
                //);
                //"An unexpected error occured while processing your request."
                //context.Response.Redirect("/Index");
            //}
            
            await Task.CompletedTask;
        }
    }
}
