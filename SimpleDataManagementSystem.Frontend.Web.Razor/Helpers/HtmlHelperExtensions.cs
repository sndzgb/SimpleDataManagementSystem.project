using Microsoft.AspNetCore.Mvc.Rendering;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Helpers
{
    public static class HtmlHelperExtensions
    {
        public static string ActiveClass(this IHtmlHelper htmlHelper, string routeStartsWith)
        {
            var routeData = htmlHelper.ViewContext.RouteData;

            var pageRoute = routeData.Values["page"]?.ToString();

            return pageRoute!.StartsWith(routeStartsWith) ? "active-navbar-link" : "";
        }
    }
}
