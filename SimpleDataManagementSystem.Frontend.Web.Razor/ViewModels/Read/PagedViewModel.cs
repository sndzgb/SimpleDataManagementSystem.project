using Microsoft.AspNetCore.Mvc;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Read
{
    public class PagedViewModel
    {
        [BindProperty(Name = "pagenr", SupportsGet = true)]
        public int PageNr { get; set; } = 1;

        [BindProperty(Name = "ipp", SupportsGet = true)]
        public int ItemsPerPage { get; set; } = 8;
    }
}
