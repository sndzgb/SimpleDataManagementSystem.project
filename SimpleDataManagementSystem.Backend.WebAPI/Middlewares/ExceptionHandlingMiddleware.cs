using System.Net.Mime;
using System.Net;
using System.Text.Json;
using SimpleDataManagementSystem.Backend.Logic.Exceptions;
using Microsoft.Data.SqlClient;
using SimpleDataManagementSystem.Backend.WebAPI.WebApiModels;

namespace SimpleDataManagementSystem.Backend.WebAPI.Middlewares
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
            string message = "Unexpected error occured.";
            int httpStatusCode = (int)HttpStatusCode.InternalServerError;
            List<string> details = new List<string>();

            var type = ex.GetType();


            // -- framework exceptions START --
            if (type == typeof(InvalidOperationException))
            {
                message = "Unexpected error occured while processing your request.";
                httpStatusCode = (int)HttpStatusCode.InternalServerError;
            }

            if (type == typeof(ArgumentNullException))
            {
                message = string.IsNullOrEmpty(ex.Message) ? "Request parameters not valid." : ex.Message;
                httpStatusCode = (int)HttpStatusCode.BadRequest;
            }

            if (type == typeof(OperationCanceledException))
            {
                message = string.IsNullOrEmpty(ex.Message) ? "Request was cancelled." : ex.Message;
                httpStatusCode = (int)HttpStatusCode.NoContent;
            }
            
            if (type == typeof(TaskCanceledException))
            {
                message = "Request was cancelled.";
                httpStatusCode = (int)HttpStatusCode.NoContent;
            }

            if (type == typeof(SqlException))
            {
                // TODO log
                message = "Error occured while processing your request.";
                httpStatusCode = (int)HttpStatusCode.InternalServerError;
            }
            // -- framework exceptions END --


            // -- custom exceptions START --
            if (type == typeof(NotAllowedException))
            {
                message = ex.Message;
                httpStatusCode = (int)HttpStatusCode.BadRequest;
            }

            if (type == typeof(RecordExistsException))
            {
                message = ex.Message;
                httpStatusCode = (int)HttpStatusCode.BadRequest;
            }

            if (type == typeof(RequiredRecordNotFoundException))
            {
                message = "Required record was not found.";
                httpStatusCode = (int)HttpStatusCode.BadRequest;
            }
            // -- custom exceptions END --


            if (type == typeof(Exception))
            {
                message = "An error occured while processing your request.";
            }

            context.Response.ContentType = MediaTypeNames.Application.Json;
            context.Response.StatusCode = httpStatusCode;

            var response = new ErrorWebApiModel(httpStatusCode, message, details);
            var options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            var json = JsonSerializer.Serialize(response, options);
            await context.Response.WriteAsync(json);
            await context.Response.CompleteAsync();
        }
    }
}
