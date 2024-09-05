using System.Text.Json.Serialization;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Read
{
    public class CategoriesViewModel
    {
        [JsonPropertyName("categories")]
        public List<CategoryViewModel> Categories { get; set; }

        [JsonPropertyName("pageInfo")]
        public PagedViewModel PageInfo { get; set; }
    }
}
