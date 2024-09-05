using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleDataManagementSystem.Frontend.Web.Razor.Exceptions;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Read;
using System.Diagnostics;
using System.Net;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Pages
{
    // TODO rename into "UnhandledErrorModel"

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [IgnoreAntiforgeryToken]
    public class ErrorModel : PageModel
    {
        public ErrorModel()
        {
            Error = new ErrorViewModel((int)HttpStatusCode.InternalServerError, "An unexpected error occured while processing your request.", null);
        }


        public ErrorViewModel? Error { get; set; }


        public void OnGet()
        {
            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            var exception = exceptionHandlerPathFeature.Error;

            //var pc = PageContext;
            //var hc = HttpContext;

            if (exceptionHandlerPathFeature?.Error is WebApiCallException)
            {
                var apiException = exception as WebApiCallException;
                Error = new ErrorViewModel(
                    apiException.Error.StatusCode,
                    apiException.Error.Message,
                    apiException.Error.Errors
                );

                //ViewData["Error"] = "Test error";
                //return RedirectToPage("/Items/Create/1");
                return;
            }

            // TODO handle other errors...

            return;
            //return new ObjectResult(null) { StatusCode = StatusCodes.Status500InternalServerError };
        }

        public void OnPost()
        {
            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            var exception = exceptionHandlerPathFeature.Error;


            //var pc = PageContext;
            //var hc = HttpContext;


            if (exceptionHandlerPathFeature?.Error is WebApiCallException)
            {
                var apiException = exception as WebApiCallException;
                Error = new ErrorViewModel(
                    apiException.Error.StatusCode,
                    apiException.Error.Message,
                    apiException.Error.Errors
                );

                return;
            }

            // TODO handle other errors... - server unavailable, serialization/ deserialization errors, ...

            return;
        }
    }
}