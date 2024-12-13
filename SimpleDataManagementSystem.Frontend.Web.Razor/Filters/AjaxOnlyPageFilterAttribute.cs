using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Filters
{
    public class AjaxOnlyPageFilterAttribute : Attribute, IAsyncPageFilter
    {
        public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {
            if (context.HttpContext.Request.Headers["X-Requested-With"] != "XMLHttpRequest")
            {
                context.Result = new ObjectResult(null)
                {
                    StatusCode = StatusCodes.Status404NotFound
                };
            } 
            else
            {
                await next();
            }
        }

        public Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
        {
            return Task.CompletedTask;
        }
    }
}
