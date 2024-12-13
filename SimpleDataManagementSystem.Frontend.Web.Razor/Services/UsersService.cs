using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleDataManagementSystem.Frontend.Web.Razor.Exceptions;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Request;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Response;
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


        public async Task<int> CreateUserAsync(CreateUserViewModel createUserViewModel, CancellationToken cancellationToken)
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.HttpClients.SimpleDataManagementSystemHttpClient.Name);

            var content = new StringContent(JsonSerializer.Serialize(createUserViewModel), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("api/users", content, cancellationToken);
            
            if (!response.IsSuccessStatusCode)
            {
                using var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken);

                var message = await JsonSerializer.DeserializeAsync<ErrorViewModel>(contentStream);

                throw new WebApiCallException(message);
            }
            else
            {
                using var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken);

                //await response.Content.ReadAsStringAsync()

                var responseContent = await JsonSerializer.DeserializeAsync<int>(contentStream);

                return Convert.ToInt32(responseContent);
            }
        }

        public async Task DeleteUserAsync(int userId, CancellationToken cancellationToken)
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.HttpClients.SimpleDataManagementSystemHttpClient.Name);

            var response = await httpClient.DeleteAsync($"api/users/{userId}", cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                using var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken);

                var message = await JsonSerializer.DeserializeAsync<ErrorViewModel>(contentStream);

                throw new WebApiCallException(message);
            }
            else
            {
                await Task.CompletedTask;
            }
        }

        public async Task<GetMultipleUsersResponseViewModel> GetMultipleUsersAsync(CancellationToken cancellationToken, int? take = 8, int? page = 1)
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.HttpClients.SimpleDataManagementSystemHttpClient.Name);

            var response = await httpClient.GetAsync($"/api/users?take={take}&page={page}", cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                using var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken);

                var message = await JsonSerializer.DeserializeAsync<ErrorViewModel>(contentStream);

                throw new WebApiCallException(message);
            }
            else
            {
                var json = await response.Content.ReadAsStringAsync(cancellationToken);
                var responseContent = JsonSerializer.Deserialize<GetMultipleUsersResponseViewModel>(json);
                return responseContent;
            }
        }

		public async Task<GetSingleUserResponseViewModel> GetSingleUserAsync(int userId, CancellationToken cancellationToken)
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.HttpClients.SimpleDataManagementSystemHttpClient.Name);

            var response = await httpClient.GetAsync($"/api/users/{userId}", cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                using var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken);

                var message = await JsonSerializer.DeserializeAsync<ErrorViewModel>(contentStream);

                throw new WebApiCallException(message);
            }
            else
            {
                var json = await response.Content.ReadAsStringAsync(cancellationToken);
                var responseContent = JsonSerializer.Deserialize<GetSingleUserResponseViewModel>(json);
                return responseContent;
            }
        }

        public async Task UpdatePasswordAsync(int userId, UpdatePasswordViewModel updatePasswordViewModel, CancellationToken cancellationToken)
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.HttpClients.SimpleDataManagementSystemHttpClient.Name);

            var content = new StringContent(JsonSerializer.Serialize(updatePasswordViewModel), Encoding.UTF8, "application/json");

            var response = await httpClient.PutAsync($"api/users/{userId}/password", content, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                using var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken);

                var message = await JsonSerializer.DeserializeAsync<ErrorViewModel>(contentStream);

                throw new WebApiCallException(message);
            }
            else
            {
                await Task.CompletedTask;
            }
        }

        public async Task UpdateUserAsync(int userId, UpdateUserViewModel updateUserViewModel, CancellationToken cancellationToken)
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.HttpClients.SimpleDataManagementSystemHttpClient.Name);

            var content = new StringContent(JsonSerializer.Serialize(updateUserViewModel), Encoding.UTF8, "application/json");

            var response = await httpClient.PutAsync($"api/users/{userId}", content, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                using var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken);

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
