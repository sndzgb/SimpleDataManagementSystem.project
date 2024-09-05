using System.Text.Json.Serialization;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Read
{
    public class ItemsViewModel
    {
        [JsonPropertyName("items")]
        public List<ItemViewModel> Items { get; set; }
        
        [JsonPropertyName("pageInfo")]
        public PagedViewModel PageInfo { get; set; }
    }
}
