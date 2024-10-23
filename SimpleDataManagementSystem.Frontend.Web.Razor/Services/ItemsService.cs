using SimpleDataManagementSystem.Frontend.Web.Razor.Exceptions;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Read;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Write;
using System.Net.Mime;
using System.Text.Json;
using System.Text;
using System.Net.Http.Headers;
using System.Net.Http;
using SimpleDataManagementSystem.Frontend.Web.Razor.Extensions;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Services
{
    public class ItemsService : IItemsService
    {
        private readonly IHttpClientFactory _httpClientFactory;


        public ItemsService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        

        public async Task<string> AddNewItemAsync(NewItemViewModel newItemViewModel)
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.HttpClients.SimpleDataManagementSystemHttpClient.Name);

            var content = new MultipartFormDataContent();

            if (newItemViewModel.URLdoslike != null)
            {
                var fileContent = new StreamContent(newItemViewModel.URLdoslike.OpenReadStream());
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(newItemViewModel.URLdoslike.ContentType);

                content.Add(fileContent, "URLdoslike", newItemViewModel.URLdoslike.FileName);
            }

            content.Add(new StringContent(newItemViewModel.Nazivproizvoda, Encoding.UTF8, MediaTypeNames.Text.Plain), "nazivproizvoda");
            content.Add(new StringContent(newItemViewModel.Datumakcije ?? string.Empty, Encoding.UTF8, MediaTypeNames.Text.Plain), "datumakcije");
            content.Add(new StringContent(newItemViewModel.Cijena.ToString(), Encoding.UTF8, MediaTypeNames.Text.Plain), "cijena");
            content.Add(new StringContent(Convert.ToString(newItemViewModel.Kategorija), Encoding.UTF8, MediaTypeNames.Text.Plain), "kategorija");
            content.Add(new StringContent(Convert.ToString(newItemViewModel.RetailerId), Encoding.UTF8, MediaTypeNames.Text.Plain), "retailerId");
            content.Add(new StringContent(newItemViewModel.Opis, Encoding.UTF8, MediaTypeNames.Text.Plain), "opis");

            var response = await httpClient.PostAsync("api/items", content);


            // TODO finish error handling!
            //var r = await response.HandleResponseAsync<string>();
            
            return await response.HandleResponseAsync<string>();

            //await response.HandleIfInvalidResponseAsync();

            //var j = await response.Content.ReadAsStringAsync();
            //return j;

            //if (!response.IsSuccessStatusCode)
            //{
            //    var json = await response.Content.ReadAsStringAsync();
            //    var message = JsonSerializer.Deserialize<ErrorViewModel>(json);
            //    throw new WebApiCallException(message);
            //}
            //else
            //{
            //    var j = await response.Content.ReadAsStringAsync();
            //    return j;
            //}
        }

        public async Task DeleteItemAsync(string itemId)
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.HttpClients.SimpleDataManagementSystemHttpClient.Name);

            var response = await httpClient.DeleteAsync($"/api/items/{Uri.EscapeDataString(itemId)}");

            if (!response.IsSuccessStatusCode)
            {
                using var contentStream = await response.Content.ReadAsStreamAsync();

                var message = await JsonSerializer.DeserializeAsync<ErrorViewModel>(contentStream);

                throw new WebApiCallException(message);
            }
            else
            {
                return;
            }
        }

        public async Task<ItemsViewModel> GetAllItemsAsync(int? take = 8, int? page = 1)
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.HttpClients.SimpleDataManagementSystemHttpClient.Name);

            var response = await httpClient.GetAsync($"/api/items?take={take}&page={page}");

            if (!response.IsSuccessStatusCode)
            {
                using var contentStream = await response.Content.ReadAsStreamAsync();

                var message = await JsonSerializer.DeserializeAsync<ErrorViewModel>(contentStream);

                throw new WebApiCallException(message);
            }
            else
            {
                var json = await response.Content.ReadAsStringAsync();
                var responseContent = JsonSerializer.Deserialize<ItemsViewModel>(json);
                return responseContent;
            }
        }

        public async Task<ItemViewModel> GetItemByIdAsync(string itemId)
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.HttpClients.SimpleDataManagementSystemHttpClient.Name);

            var response = await httpClient.GetAsync($"/api/items/{Uri.EscapeDataString(itemId)}");

            if (!response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var error = JsonSerializer.Deserialize<ErrorViewModel>(json);

                throw new WebApiCallException(error);
            }
            else
            {
                var json = await response.Content.ReadAsStringAsync();
                var responseContent = JsonSerializer.Deserialize<ItemViewModel>(json);
                return responseContent;
            }
        }

        public async Task UpdateItemPartialAsync(string itemId)
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.HttpClients.SimpleDataManagementSystemHttpClient.Name);

            var response = await httpClient.PatchAsync($"/api/items/{Uri.EscapeDataString(itemId)}", null);

            if(!response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var error = JsonSerializer.Deserialize<ErrorViewModel>(json);
                throw new WebApiCallException(error);

            }
            else
            {
                return;
            }
        }

        public async Task UpdateItemAsync(string itemId, UpdateItemViewModel updateItemViewModel)
        {
            var httpClient = _httpClientFactory.CreateClient(Constants.HttpClients.SimpleDataManagementSystemHttpClient.Name);

            var content = new MultipartFormDataContent();

            if (updateItemViewModel.URLdoslike != null)
            {
                var fileContent = new StreamContent(updateItemViewModel.URLdoslike.OpenReadStream());
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(updateItemViewModel.URLdoslike.ContentType);

                content.Add(fileContent, "URLdoslike", updateItemViewModel.URLdoslike.FileName);
            }

            content.Add(new StringContent(updateItemViewModel.Datumakcije, Encoding.UTF8, MediaTypeNames.Text.Plain), "datumakcije");
            content.Add(new StringContent(updateItemViewModel.Cijena.ToString(), Encoding.UTF8, MediaTypeNames.Text.Plain), "cijena");
            content.Add(new StringContent(Convert.ToString(updateItemViewModel.Kategorija), Encoding.UTF8, MediaTypeNames.Text.Plain), "kategorija");
            content.Add(new StringContent(Convert.ToString(updateItemViewModel.RetailerId), Encoding.UTF8, MediaTypeNames.Text.Plain), "retailerId");
            content.Add(new StringContent(updateItemViewModel.Opis, Encoding.UTF8, MediaTypeNames.Text.Plain), "opis");

            var response = await httpClient.PutAsync($"api/items/{Uri.EscapeDataString(itemId)}", content);

            if (!response.IsSuccessStatusCode)
            {
                using var contentStream = await response.Content.ReadAsStreamAsync();

                var message = await JsonSerializer.DeserializeAsync<ErrorViewModel>(contentStream);

                throw new WebApiCallException(message);
            }
            else
            {
                return;
            }
        }
    }
}
