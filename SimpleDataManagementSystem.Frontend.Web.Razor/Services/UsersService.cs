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
            //try
            //{
                var httpClient = _httpClientFactory.CreateClient("SimpleDataManagementSystemHttpClient");

                var content = new StringContent(JsonSerializer.Serialize(newUserViewModel), Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync("api/users", content);
            //    return 1;
            //}
            //catch (Exception e)
            //{
            //}
            //return 1;

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
            var httpClient = _httpClientFactory.CreateClient("SimpleDataManagementSystemHttpClient");

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

        public async Task<List<UserViewModel>> GetAllUsersAsync(int? take = 8, int? page = 1)
        {
            var httpClient = _httpClientFactory.CreateClient("SimpleDataManagementSystemHttpClient");

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
                var responseContent = JsonSerializer.Deserialize<List<UserViewModel>>(json);
                return responseContent;

                //using var contentStream = await response.Content.ReadAsStreamAsync();

                //var responseContent = await JsonSerializer.DeserializeAsync<List<UserViewModel>>(contentStream);

                //return responseContent;
            }
        }

        public async Task<UserViewModel> GetUserByIdAsync(int userId)
        {
            var httpClient = _httpClientFactory.CreateClient("SimpleDataManagementSystemHttpClient");

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

        public async Task UpdateUserAsync(int userId, UpdateUserViewModel updateUserViewModel)
        {
            var httpClient = _httpClientFactory.CreateClient("SimpleDataManagementSystemHttpClient");

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
