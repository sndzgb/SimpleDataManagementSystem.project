using System.Text.Json.Serialization;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Read
{
    public class UserLogInResponseViewModel
    {
        [JsonPropertyName("claims")]
        public List<MyClaimViewModel> Claims { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; }

        public class MyClaimViewModel
        {
            [JsonPropertyName("key")]
            public string Key { get; set; }

            [JsonPropertyName("value")]
            public string Value { get; set; }
        }
    }
}
