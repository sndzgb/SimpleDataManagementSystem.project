using Microsoft.AspNetCore.Mvc;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Pages.ViewComponents
{
    public class SnackbarViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
