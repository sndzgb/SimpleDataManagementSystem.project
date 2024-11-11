using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Read;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Write;
using System.Text.Json;
using System.Text;
using SimpleDataManagementSystem.Frontend.Web.Razor.Exceptions;
using System.Net;
using SimpleDataManagementSystem.Frontend.Web.Razor.Extensions;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Services
{
    public class AccountsService : IAccountsService
    {
        private readonly IHttpClientFactory _httpClientFactory;


        public AccountsService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        public async Task<AuthTokenViewModel?> LogInAsync(string username, string password)
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

            // THIS
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return null;
            }

            // USE THIS FOR ALL METHODS BUT THIS ONE!!
            return await response.HandleResponseAsync<AuthTokenViewModel>();
            //await response.HandleIfInvalidResponseAsync();
            //response.EnsureSuccessStatusCode();


            // token should not be null here
            //var json = await response.Content.ReadAsStringAsync();

            //if (json == null || string.IsNullOrEmpty(json))
            //{
            //    return null;
            //}

            //return json;

            
            //HandleFailedResponse(response);


            //if (!response.IsSuccessStatusCode)
            //{
            //    //if (response.StatusCode == HttpStatusCode.Unauthorized)
            //    //{
            //    //    return null;
            //    //}

            //    using var contentStream = await response.Content.ReadAsStreamAsync();

            //    var message = await JsonSerializer.DeserializeAsync<ErrorViewModel>(contentStream);

            //    throw new WebApiCallException(message);
            //}
            //else
            //{
            //    var json = await response.Content.ReadAsStringAsync();

            //    if (json == null || string.IsNullOrEmpty(json))
            //    {
            //        return null;
            //    }

            //    return json;
            //}
        }

        public async Task<UserViewModel?> GetAccountDetailsAsync()
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.HttpClients.SimpleDataManagementSystemHttpClient.Name);

            var response = await httpClient.GetAsync($"/api/accounts/details");

            if (!response.IsSuccessStatusCode)
            {
                using var contentStream = await response.Content.ReadAsStreamAsync();

                var message = await JsonSerializer.DeserializeAsync<ErrorViewModel>(contentStream);

                throw new WebApiCallException(message);
            }
            else
            {
                var json = await response.Content.ReadAsStringAsync();
                var responseContent = JsonSerializer.Deserialize<UserViewModel>(json);
                return responseContent;
            }
        }

        public async Task UpdatePasswordAsync(string oldPassword, string newPassword)
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.HttpClients.SimpleDataManagementSystemHttpClient.Name);

            var content = new StringContent(
                JsonSerializer.Serialize(
                    new { oldPassword = oldPassword, newPassword = newPassword }
                ), 
                Encoding.UTF8, "application/json"
            );

            var response = await httpClient.PutAsync($"api/accounts/password", content);

            if (!response.IsSuccessStatusCode)
            {
                using var contentStream = await response.Content.ReadAsStreamAsync();

                var message = await JsonSerializer.DeserializeAsync<ErrorViewModel>(contentStream);

                throw new WebApiCallException(message);
            }
            else
            {
                await Task.CompletedTask;
            }
        }
    }
}
