using Ganss.Xss;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Helpers
{
    public static class RawHtmlHelpers
    {
        public static string SanitizeHtmlString(string html)
        {
            var sanitizer = new HtmlSanitizer();
            return sanitizer.Sanitize(html);
        }
    }
}
