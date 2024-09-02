using System.Text.Json.Serialization;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Read
{
    public class CategoryViewModel
    {
        [JsonPropertyName("id")]
        public int ID { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("priority")]
        public int Priority { get; set; }
    }
}
