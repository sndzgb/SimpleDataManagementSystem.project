using System.Text.Json.Serialization;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Read
{
    public class UsersViewModel
    {
        [JsonPropertyName("users")]
        public List<UserViewModel> Users { get; set; }

        [JsonPropertyName("pageInfo")]
        public PagedViewModel PageInfo { get; set; }
    }
}
