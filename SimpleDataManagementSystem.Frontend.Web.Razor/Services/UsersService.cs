using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleDataManagementSystem.Frontend.Web.Razor.Exceptions;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Read;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Write;
using System.Text;
using System.Text.Json;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Services
{
    public class UsersService : IUsersService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        
        public UsersService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        public async Task<int> AddNewUserAsync(NewUserViewModel newUserViewModel)
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.HttpClients.SimpleDataManagementSystemHttpClient.Name);

            var content = new StringContent(JsonSerializer.Serialize(newUserViewModel), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("api/users", content);
            
            if (!response.IsSuccessStatusCode)
            {
                using var contentStream = await response.Content.ReadAsStreamAsync();

                var message = await JsonSerializer.DeserializeAsync<ErrorViewModel>(contentStream);

                throw new WebApiCallException(message);
            }
            else
            {
                using var contentStream = await response.Content.ReadAsStreamAsync();

                //await response.Content.ReadAsStringAsync()

                var responseContent = await JsonSerializer.DeserializeAsync<int>(contentStream);

                return Convert.ToInt32(responseContent);
            }
        }

        public async Task DeleteUserAsync(int userId)
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.HttpClients.SimpleDataManagementSystemHttpClient.Name);

            var response = await httpClient.DeleteAsync($"api/users/{userId}");

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

        public async Task<UsersViewModel> GetAllUsersAsync(int? take = 8, int? page = 1)
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.HttpClients.SimpleDataManagementSystemHttpClient.Name);

            var response = await httpClient.GetAsync($"/api/users?take={take}&page={page}");

            if (!response.IsSuccessStatusCode)
            {
                using var contentStream = await response.Content.ReadAsStreamAsync();

                var message = await JsonSerializer.DeserializeAsync<ErrorViewModel>(contentStream);

                throw new WebApiCallException(message);
            }
            else
            {
                var json = await response.Content.ReadAsStringAsync();
                var responseContent = JsonSerializer.Deserialize<UsersViewModel>(json);
                return responseContent;
            }
        }

        public async Task<UserViewModel> GetUserByIdAsync(int userId)
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.HttpClients.SimpleDataManagementSystemHttpClient.Name);

            var response = await httpClient.GetAsync($"/api/users/{userId}");

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

        public async Task UpdatePasswordAsync(int userId, UpdatePasswordViewModel updatePasswordViewModel)
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.HttpClients.SimpleDataManagementSystemHttpClient.Name);

            var content = new StringContent(JsonSerializer.Serialize(updatePasswordViewModel), Encoding.UTF8, "application/json");

            var response = await httpClient.PutAsync($"api/users/{userId}/password", content);

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

        public async Task UpdateUserAsync(int userId, UpdateUserViewModel updateUserViewModel)
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.HttpClients.SimpleDataManagementSystemHttpClient.Name);

            var content = new StringContent(JsonSerializer.Serialize(updateUserViewModel), Encoding.UTF8, "application/json");

            var response = await httpClient.PutAsync($"api/users/{userId}", content);

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
