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

            // THIS
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return null;
            }



            // FOR ALL METHODS BUT THIS!!
            return await response.HandleResponseAsync<string>();
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
    }
}
