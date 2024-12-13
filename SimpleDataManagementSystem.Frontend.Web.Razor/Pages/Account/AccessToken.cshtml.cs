using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleDataManagementSystem.Frontend.Web.Razor.Filters;
using SimpleDataManagementSystem.Shared.Common.Constants;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Pages.Account
{
    [AjaxOnlyPageFilterAttribute]
    public class AccessTokenModel : PageModel
    {
        public async Task<IActionResult> OnGetToken()
        {
            //await HttpContext.GetTokenAsync("access_token")
            var accessToken = HttpContext.User?.Claims?.Where(x => x.Type == ExtendedClaims.Type.Jwt).FirstOrDefault()?.Value;
            
            return new JsonResult(accessToken)
            {
                StatusCode = StatusCodes.Status200OK
            };
        }
    }
}
