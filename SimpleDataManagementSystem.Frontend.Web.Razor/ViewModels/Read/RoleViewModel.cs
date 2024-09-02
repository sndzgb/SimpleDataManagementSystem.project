using System.Text.Json.Serialization;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Read
{
    public class RoleViewModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
