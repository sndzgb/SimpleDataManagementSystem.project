﻿using System.Net.Mime;
using System.Net;
using System.Text.Json;
using SimpleDataManagementSystem.Backend.Logic.Exceptions;
using SimpleDataManagementSystem.Backend.WebAPI.WebApiModels.Read;

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
            string message = string.Empty;
            int httpStatusCode = (int)HttpStatusCode.InternalServerError;
            List<string> details = new List<string>();

            var type = ex.GetType();

            if (type == typeof(NotAllowedException))
            {
                message = ex.Message;
                httpStatusCode = (int)HttpStatusCode.BadRequest;
            }

            if (type == typeof(Exception))
            {
                message = "An error occured while processing your request.";
            }

            context.Response.ContentType = MediaTypeNames.Application.Json;
            context.Response.StatusCode = httpStatusCode;

            var response = new ErrorWebApiModel(httpStatusCode, message, details);
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            var json = JsonSerializer.Serialize(response, options);
            await context.Response.WriteAsync(json);
        }
    }
}
