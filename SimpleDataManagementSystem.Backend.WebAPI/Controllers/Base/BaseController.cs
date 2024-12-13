using Microsoft.AspNetCore.Mvc;

namespace SimpleDataManagementSystem.Backend.WebAPI.Controllers.Base
{
    [Controller]
    public abstract class BaseController : ControllerBase
    {
        protected BaseController()
        {
        }

        protected int GetUserId()
        {
            return Convert.ToInt32(HttpContext.User.Identity?.Name);
        }
    }
}
