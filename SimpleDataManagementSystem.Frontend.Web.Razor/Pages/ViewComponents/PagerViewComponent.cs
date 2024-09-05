using Microsoft.AspNetCore.Mvc;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Read;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Pages.ViewComponents
{
    public class PagerViewComponent : ViewComponent
    {
        public Paged Paged { get; set; }


        public IViewComponentResult Invoke(PagedViewModel pageInfo)
        {
            Paged = new Paged(pageInfo);
            return View(Paged);
        }
    }

    public class Paged
    {
        public Paged(PagedViewModel pagedViewModel)
        {
            this.PageInfo = pagedViewModel;
        }

        public PagedViewModel PageInfo { get; private set; }
    }
}
