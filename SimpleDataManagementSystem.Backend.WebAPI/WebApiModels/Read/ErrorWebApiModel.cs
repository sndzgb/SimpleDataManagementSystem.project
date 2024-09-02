using System.Text.Json.Serialization;

namespace SimpleDataManagementSystem.Backend.WebAPI.WebApiModels.Read
{
    public class ErrorWebApiModel
    {
        [JsonPropertyName("statusCode")]
        public int StatusCode { get; private set; }

        [JsonPropertyName("message")]
        public string? Message { get; private set; }

        [JsonPropertyName("details")]
        public string? Details { get; private set; }

        [JsonPropertyName("errors")]
        public List<string> Errors { get; private set; }

        public ErrorWebApiModel(int statusCode, string? message, List<string>? errors = null)
        {
            StatusCode = statusCode;
            Message = message;
            Errors = errors;
        }

        //public ErrorWebApiModel(int statusCode, string? message, string? details = null)
        //{
        //    StatusCode = statusCode;
        //    Message = message;
        //    Details = details;
        //}
    }
}
