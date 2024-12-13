using System.Text.Json.Serialization;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Response
{
    public class LogInResponseViewModel
    {
        [JsonPropertyName("jwt")]
        public string? Jwt { get; set; }
    }
}
