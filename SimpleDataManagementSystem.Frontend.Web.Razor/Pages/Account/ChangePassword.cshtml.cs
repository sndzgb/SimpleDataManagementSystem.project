using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleDataManagementSystem.Frontend.Web.Razor.Exceptions;
using SimpleDataManagementSystem.Frontend.Web.Razor.Services;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Write;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Pages.Account
{
    [ValidateAntiForgeryToken]
    public class ChangePasswordModel : PageModel
    {
        private readonly IUsersService _usersService;


        public ChangePasswordModel(IUsersService usersService)
        {
            _usersService = usersService;
        }


        //[FromRoute]
        [BindProperty(SupportsGet = true)]
        public int? UserId { get; set; }

        [BindProperty]
        public UpdatePasswordViewModel UpdatePasswordViewModel { get; set; }

        public WebApiCallException Error { get; set; }


        public async Task<IActionResult> OnGet()
        {
            //UserId = (int?)ViewData["UserId"];

            //// in case user refreshes the page, ViewData will be lost
            //if (UserId == null)
            //{
            //    //RedirectToAction("/Login");
            //    RedirectToPage("/Account/Login");
            //    return;
            //}

            return null;
        }

        public async Task<IActionResult> OnPost()
        {
            try
            {
                if (UserId == null)
                {   
                    return RedirectToPage("/Account/Login");
                }

                if (!ModelState.IsValid)
                {
                    return await OnGet();
                }

                await _usersService.UpdatePasswordAsync((int)UserId!, UpdatePasswordViewModel);

                return LocalRedirect("/");
                //return RedirectToPage("/Login");
            }
            catch (WebApiCallException wace)
            {
                Error = wace; // set error on model
                return await OnGet();
            }
        }
    }
}
