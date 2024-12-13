using System.Text.Json;
using System.Text.Json.Serialization;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels
{
    public class ErrorViewModel
    {
        [JsonPropertyName("statusCode")]
        public int StatusCode { get; /*private*/ set; }

        [JsonPropertyName("message")]
        public string? Message { get; /*private*/ set; }

        [JsonPropertyName("errors")]
        public List<string> Errors { get; /*private*/ set; }

        public ErrorViewModel(int statusCode, string? message, List<string>? errors = null)
        {
            StatusCode = statusCode;
            Message = message;
            Errors = errors;
        }

        /*
            TODO check:
            Could not create an instance of type 'SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Read.ErrorViewModel'. 
        Model bound complex types must not be abstract or value types and must have a parameterless constructor. 
        Record types must have a single primary constructor. 
        Alternatively, set the 'Model' property to a non-null value in 
            the 'SimpleDataManagementSystem.Frontend.Web.Razor.Pages.ErrorModel' constructor.
        */
        public ErrorViewModel()
        {

        }

        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
