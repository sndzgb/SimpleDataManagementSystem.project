using System.Text.Json.Serialization;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Read
{
    public class ErrorViewModel
    {
        [JsonPropertyName("statusCode")]
        public int StatusCode { get; private set; }

        [JsonPropertyName("message")]
        public string? Message { get; private set; }

        [JsonPropertyName("errors")]
        public List<string> Errors { get; private set; }

        public ErrorViewModel(int statusCode, string? message, List<string>? errors = null)
        {
            StatusCode = statusCode;
            Message = message;
            Errors = errors;
        }
    }
}
