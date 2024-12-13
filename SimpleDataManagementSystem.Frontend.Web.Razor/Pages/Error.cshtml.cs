using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleDataManagementSystem.Frontend.Web.Razor.Exceptions;
using SimpleDataManagementSystem.Frontend.Web.Razor.Pages.Base;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels;
using System.Diagnostics;
using System.Net;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Pages
{
    // TODO rename into "UnhandledErrorModel" - handles uncaught/ unhandled exceptions

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [IgnoreAntiforgeryToken]
    public class ErrorModel : BasePageModel<ErrorViewModel> // PageModel
    {
        public ErrorModel()
        {
            Error = new ErrorViewModel((int)HttpStatusCode.InternalServerError, "An unexpected error occured while processing your request.", null);
        }


        // USE MIDDLEWARE FOR EXCEPTIONS & handling, THIS FOR showing error HttpStatusCodes -- Call web api and return 500, 400, 401, 403, 404, ...
        public IActionResult OnGet(int? statusCode)
        {
            var error = HttpContext.Items.TryGetValue(nameof(ErrorViewModel), out object? r);
            ErrorViewModel? result = (ErrorViewModel?)r;

            /*
                true if:
                    1) Client - "AuthorizationService.AuthorizeAsync()" fails
                    2) Server - returns "AuthorizationService.AuthorizeAsync" failure
            */
            if (statusCode == StatusCodes.Status403Forbidden)
            {
                return RedirectToPage("/Forbidden");
            }
            
            if (statusCode == StatusCodes.Status404NotFound)
            {
                Error = new ErrorViewModel(
                    StatusCodes.Status404NotFound,
                    result?.Message ?? "Resource not found.",
                    result?.Errors
                );
            }

            if (statusCode == StatusCodes.Status400BadRequest)
            {
                Error = new ErrorViewModel(
                    StatusCodes.Status400BadRequest,
                    result?.Message ?? "Bad request occured.",
                    result?.Errors
                );
            }
            
            if (statusCode == StatusCodes.Status401Unauthorized)
            {
                return RedirectToPage("/Account/PasswordChange");
            }
            
            if (statusCode == StatusCodes.Status500InternalServerError)
            {
                // Get error message from HttpContext.Items
                Error = new ErrorViewModel(
                    StatusCodes.Status500InternalServerError, 
                    "Something went wrong while processing your request.", 
                    null
                );
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            var exception = exceptionHandlerPathFeature.Error;

            var type = exception.GetType();

            var request = HttpContext.Request;

            if (type == typeof(UnauthorizedException)) // login
            {
                return base.RedirectToPage("/Account/Login");
            }

            if (type == typeof(ForbiddenException)) // not allowed
            {
                // redirect?
                Error = new ErrorViewModel(403, "Forbidden.", null);
                return null;
            }


            // default
            Error = new ErrorViewModel(500, "Unknown error occured while processing your request.");
            return null;
        }
    }
}