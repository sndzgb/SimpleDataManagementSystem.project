using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Read;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Write;
using System.Text.Json;
using System.Text;
using SimpleDataManagementSystem.Frontend.Web.Razor.Exceptions;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Services
{
    public class AccountsService : IAccountsService
    {
        private readonly IHttpClientFactory _httpClientFactory;


        public AccountsService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        public async Task<string?> LogInAsync(string username, string password)
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.HttpClients.SimpleDataManagementSystemHttpClient.Name);

            var content = new StringContent(
                JsonSerializer.Serialize(new UserLogInRequestViewModel()
                {
                    Username = username,
                    Password = password
                }), 
                Encoding.UTF8,
                "application/json"
            );

            var response = await httpClient.PostAsync("api/accounts", content);

            if (!response.IsSuccessStatusCode)
            {
                using var contentStream = await response.Content.ReadAsStreamAsync();

                var message = await JsonSerializer.DeserializeAsync<ErrorViewModel>(contentStream);

                throw new WebApiCallException(message);
            }
            else
            {
                var json = await response.Content.ReadAsStringAsync();

                if (json == null || string.IsNullOrEmpty(json))
                {
                    return null;
                }

                return json;
            }
        }
    }
}
