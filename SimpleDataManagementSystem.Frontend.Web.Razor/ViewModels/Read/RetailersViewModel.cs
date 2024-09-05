using System.Text.Json.Serialization;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Read
{
    public class RetailersViewModel
    {
        [JsonPropertyName("retailers")]
        public List<RetailerViewModel> Retailers { get; set; }

        [JsonPropertyName("pageInfo")]
        public PagedViewModel PageInfo { get; set; }
    }
}
