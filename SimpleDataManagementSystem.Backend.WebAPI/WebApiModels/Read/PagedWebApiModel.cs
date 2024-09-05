using System.Text.Json.Serialization;

namespace SimpleDataManagementSystem.Backend.WebAPI.WebApiModels.Read
{
    public class PagedWebApiModel
    {
        [JsonPropertyName("total")]
        public int Total { get; set; }
        
        [JsonPropertyName("page")]
        public int Page { get; set; }
        
        [JsonPropertyName("take")]
        public int Take { get; set; }
    }
}
