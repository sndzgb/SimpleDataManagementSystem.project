using Microsoft.AspNetCore.Mvc;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Pages.ViewComponents
{
    public class DisplayErrorViewComponent : ViewComponent
    {
        public ErrorViewModel Error { get; set; }


        public IViewComponentResult Invoke(ErrorViewModel errorViewModel)
        {
            Error = errorViewModel;
            return View(model: Error);
        }
    }
}
