using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using SimpleDataManagementSystem.Backend.WebAPI.WebApiModels;

namespace SimpleDataManagementSystem.Backend.WebAPI.Filters
{
    public class ValidateModelActionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Values
                                            .SelectMany(v => v.Errors)
                                            .Select(e => e.ErrorMessage).ToList();

                var model = new ErrorWebApiModel((int)HttpStatusCode.BadRequest, "One or more validation errors occured", errors);

                var serializeOptions = new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                };

                context.Result = new ObjectResult(JsonSerializer.Serialize(model, serializeOptions))
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }
        }
    }
}
