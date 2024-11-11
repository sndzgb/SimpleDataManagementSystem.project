using System.Text.Json.Serialization;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Read
{
    public class AuthTokenViewModel
    {
        [JsonPropertyName("jwt")]
        public string? Jwt { get; set; }
    }
}
