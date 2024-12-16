using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using SimpleDataManagementSystem.Frontend.Web.Razor.Exceptions;
using SimpleDataManagementSystem.Frontend.Web.Razor.Helpers;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels;
using SimpleDataManagementSystem.Shared.Common.Constants;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Pages.Base
{
    public abstract class BasePageModel<T> : PageModel
    {
        protected int GetUserId()
        {
            return Convert.ToInt32(HttpContext.User.Identity?.Name);
        }

        public ErrorViewModel? Error { get; set; }

        private T? _model;

        [BindProperty]
        public T? Model
        {
            get
            {
                return _model;
            }
            set
            {
                _model = value;
            }
        }

        // TODO use this for granular logic; Global Middleware for unhandled exceptions -- redirect to 500 page
        // TODO rename "ExceptionHandlingMiddleware" to "UnhandledExceptionHandlingMiddleware"
        public override void OnPageHandlerExecuted(PageHandlerExecutedContext context)
        {
            var myCompleteAddress = Request.Path.Value + Request.QueryString.Value;

            if (TempData["ErrorViewModel"] != null) // TODO nameof
            {
                Error = JsonSerializer.Deserialize<ErrorViewModel>(TempData["ErrorViewModel"].ToString());
            }

            if (context.Exception != null)
            {
                var exType = context.Exception.GetType();

                if (exType == typeof(WebApiCallException))
                {
                    var ex = context.Exception as WebApiCallException;
                    var error = new ErrorViewModel()
                    {
                        Errors = ex.Error.Errors,
                        Message = ex.Error.Message,
                        StatusCode = ex.Error.StatusCode
                    };

                    // TODO test!
                    if (context.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest") // ajax request
                    {
                        context.HttpContext.Response.StatusCode = 
                            ex?.Error?.StatusCode ?? StatusCodes.Status500InternalServerError;
                        context.HttpContext.Response.ContentType = "application/json";
                        
                        string json = JsonSerializer.Serialize(
                            new ErrorViewModel(
                                ex?.Error?.StatusCode ?? StatusCodes.Status500InternalServerError,
                                ex?.Error?.Message,
                                ex?.Error?.Errors
                            )
                        );

                        //context.ExceptionHandled = true;
                        context.HttpContext.Response.WriteAsync(json).GetAwaiter().GetResult();
                        context.HttpContext.Response.CompleteAsync().GetAwaiter().GetResult();
                    }

                    if (error.StatusCode == StatusCodes.Status403Forbidden)
                    {
                        context.Result = Redirect("/Forbidden");
                    }

                    if (error.StatusCode == StatusCodes.Status401Unauthorized)
                    {
                        context.Result = Redirect("/Account/Login");
                    }

                    if (error.StatusCode == StatusCodes.Status404NotFound)
                    {
                        context.Result = Redirect("/Errors/NotFound");
                    }

                    if (error.StatusCode == StatusCodes.Status400BadRequest)
                    {
                        context.Result = Redirect(myCompleteAddress);
                    }

                    context.ExceptionHandled = true;

                    TempData["ErrorViewModel"] = JsonSerializer.Serialize<ErrorViewModel>(error);
                }
            }

            base.OnPageHandlerExecuted(context);
        }


        public override async void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            if (TempData["Model"] != null) // read the data if any, so it is evicted from further requests
            {
                // this will fail silently if the "Model" is not the same as "T",
                // meaning we navigated away from form page
                _model = JsonSerializer.Deserialize<T>(TempData["Model"].ToString());
            }

            var myCompleteAddress = Request.Path.Value + Request.QueryString.Value; // Get areas + handler + ...

            var myAddress = Request.Path.Value;
            var myQuery = Request.QueryString.Value;

            var val = Request.RouteValues.Values; // + PathParm(s) + QueryString


            // if "GET", get "model" from "tempData" & bind it to "modelState"
            // get serialized errors, if any from previous "POST" call
            if (context?.HandlerMethod?.MethodInfo.Name == "OnGet")
            {
                var errors = TempData["ModelStateErrors"]?.ToString();

                if (!errors.IsNullOrEmpty())
                {
                    var errorList = JsonSerializer.Deserialize<List<ModelStateTransferValue>>(errors!);
                    var modelState = new ModelStateDictionary();

                    foreach (var item in errorList)
                    {
                        modelState.SetModelValue(item.Key, item.RawValue, item.AttemptedValue);
                        foreach (var error in item.ErrorMessages)
                        {
                            modelState.AddModelError(item.Key, error);
                        }
                    }

                    PageContext.ModelState.Merge(modelState);
                }
            }

            // TODO route/ page + path/ query parameters

            // check if "POST"?
            if (context?.HandlerMethod?.MethodInfo.Name == "OnPost")
            {
                if (!ModelState.IsValid)
                {
                    // serialize & store in TempData
                    TempData["Model"] = JsonSerializer.Serialize<T>(Model);

                    // query params + path params

                    // store errors in "ModelStateDictionary"
                    var errorList = context.ModelState
                        .Select(kvp => new ModelStateTransferValue
                        {
                            Key = kvp.Key,
                            AttemptedValue = kvp.Value.AttemptedValue,
                            RawValue = kvp.Value.RawValue,
                            ErrorMessages = kvp.Value.Errors.Select(err => err.ErrorMessage).ToList(),
                        });

                    TempData["ModelStateErrors"] = JsonSerializer.Serialize(errorList);

                    // RETURN: redirect to the same page
                    context.Result = Redirect(myCompleteAddress);
                    //context.Result = RedirectToPage("/Items/CreateItem");
                    //context.Result = RedirectToRoute(routeName: myAddress, routeValues: routeValuesDictionary);
                }
            }
        }
    }

    internal class ModelStateTransferValue
    {
        public string Key { get; set; }
        public string AttemptedValue { get; set; }
        public object RawValue { get; set; }
        public ICollection<string> ErrorMessages { get; set; } = new List<string>();
    }
}
