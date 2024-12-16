using Microsoft.AspNetCore.Mvc.Filters;
using System.IO;
using System.Text;
using System.Text.Json;

namespace SimpleDataManagementSystem.Backend.WebAPI.Filters
{
    public class LoggingActionFilter : IAsyncActionFilter, IOrderedFilter
    {
        public int Order { get; set; }


        public LoggingActionFilter()
        {
        }


        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var instance = DateTime.UtcNow.Ticks;

            using (var scope = context.HttpContext.RequestServices.CreateScope())
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<LoggingActionFilter>>();

                var request = context.HttpContext.Request;

                if (logger.IsEnabled(LogLevel.Trace))
                {
                    logger.LogTrace("Start of '{instance}'", instance);

                    var path = request.Path;
                    var queryString = request.QueryString;
                    var routeValues = request.RouteValues;

                    var payload = new Request();

#if DEVELOPMENT
                    var headers = request.Headers;
                    payload.Headers = headers;

                    //var body = await ReadAsStringAsync();
                    //payload.Body = body;

                    var hasForm = request.HasFormContentType;
                    if (hasForm)
                    {
                        // read & serialize
                        var form = await request.ReadFormAsync();
                        var serializedForm = JsonSerializer.Serialize(form);
                        payload.Form = serializedForm;

                        if (form.Files.Any())
                        {
                            var formFiles = JsonSerializer.Serialize(form.Files);
                            payload.FormFiles = formFiles;
                        }
                    }
#endif
                    payload.Path = path;
                    payload.QueryString = queryString;
                    payload.RouteValues = routeValues;

                    logger.LogTrace("Request: [{instance}] {obj}", instance, JsonSerializer.Serialize(payload));
                }

                //context.HttpContext.Request.EnableBuffering();
                //var body = request.Body;

                //var res = await ReadAsStringAsync(request.Body, false);

                //var form = request.Form;
                //if (body.CanRead)
                //{
                    //var data = new byte[request.Body.Length];
                    //request.Body.Read(data, 0, data.Length);
                    //var bodyString = Encoding.UTF8.GetString(data);
                    //logger.LogTrace();
                //}
                var result = await next();

                if (logger.IsEnabled(LogLevel.Trace))
                {
                    logger.LogTrace("End of '{instance}'", instance);
                }
                // response
                //var response = result.HttpContext.Response;
            }
        }

        private async Task<string> ReadAsStringAsync(Stream requestBody, bool leaveOpen = false)
        {
            using StreamReader reader = new(requestBody, leaveOpen: leaveOpen);
            var bodyAsString = await reader.ReadToEndAsync();
            return bodyAsString;
        }

        private class Request
        {
            //public object Body { get; set; }
            public object Form { get; set; }
            public object FormFiles { get; set; }
            public object Headers { get; set; }
            public object Path { get; set; }
            public object QueryString { get; set; }
            public object RouteValues { get; set; }
        }
    }
}
