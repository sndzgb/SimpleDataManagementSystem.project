using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleDataManagementSystem.Frontend.Web.Razor.Exceptions;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Read;
using System.Diagnostics;
using System.Net;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Pages
{
    // TODO rename into "UnhandledErrorModel" - handles uncaught/ unhandled exceptions

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [IgnoreAntiforgeryToken]
    public class ErrorModel : PageModel
    {
        public ErrorModel()
        {
            Error = new ErrorViewModel((int)HttpStatusCode.InternalServerError, "An unexpected error occured while processing your request.", null);
        }


        public ErrorViewModel? Error { get; set; }


        public IActionResult OnGet()
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
                return Page();
            }

            // TODO handle other errors...

            return Page();
            //return new ObjectResult(null) { StatusCode = StatusCodes.Status500InternalServerError };
        }

        public IActionResult OnPost()
        {
            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            var exception = exceptionHandlerPathFeature.Error;

            var type = exception.GetType();

            var request = HttpContext.Request;

            //if (request)
            //{
            //    base.LocalRedirect();
            //}


            if (type == typeof(UnauthorizedException)) // login
            {
                return base.RedirectToPage("/Account/Account");
            }

            if (type == typeof(ForbiddenException)) // not allowed
            {
                Error = new ErrorViewModel(403, "Forbidden.", null);
                return null;
                //return OnGet();
            }


            // default
            Error = new ErrorViewModel(500, "Unknown error occured while processing your request.");
            return null;
            //return OnGet();


            //if (type == typeof(HttpRequestException))
            //{
            //    if (HttpContext.Response.StatusCode == StatusCodes.Status401Unauthorized)
            //    {
            //        //HttpContext.Items.Add("Error", "Invalid login attempt.");
            //        //HttpContext.Response.Redirect("/Unauthorized", true);
                    
            //        //return Forbid();
            //        return base.RedirectToPage("/Error"); // default
            //    }
                
            //    //HttpContext.Items.Add("Error", "Invalid login attempt.");
            //    //ViewData["Error"] = "Invalid login attempt";
            //    return base.RedirectToPage("/Index");
            //    //return base.RedirectToPagePermanent("/Index");
            //}




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
                
                //return;
            }

            // TODO handle other errors... - server unavailable, serialization/ deserialization errors, ...

            //return;

            return OnGet();
            //return BadRequest();
        }
    }
}