using SimpleDataManagementSystem.Frontend.Web.Razor.Exceptions;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Read;
using SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Write;
using System.Net.Mime;
using System.Text.Json;
using System.Text;
using System.Net.Http.Headers;

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
            var httpClient = _httpClientFactory.CreateClient("SimpleDataManagementSystemHttpClient");

            var content = new MultipartFormDataContent();

            if (newItemViewModel.URLdoslike != null)
            {
                var fileContent = new StreamContent(newItemViewModel.URLdoslike.OpenReadStream());
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(newItemViewModel.URLdoslike.ContentType);

                content.Add(fileContent, "URLdoslike", newItemViewModel.URLdoslike.FileName);
            }

            content.Add(new StringContent(newItemViewModel.Nazivproizvoda, Encoding.UTF8, MediaTypeNames.Text.Plain), "nazivproizvoda");
            content.Add(new StringContent(newItemViewModel.Datumakcije, Encoding.UTF8, MediaTypeNames.Text.Plain), "datumakcije");
            content.Add(new StringContent(newItemViewModel.Cijena.ToString(), Encoding.UTF8, MediaTypeNames.Text.Plain), "cijena");
            content.Add(new StringContent(Convert.ToString(newItemViewModel.Kategorija), Encoding.UTF8, MediaTypeNames.Text.Plain), "kategorija");
            content.Add(new StringContent(newItemViewModel.Nazivretailera, Encoding.UTF8, MediaTypeNames.Text.Plain), "nazivretailera");
            content.Add(new StringContent(newItemViewModel.Opis, Encoding.UTF8, MediaTypeNames.Text.Plain), "opis");

            var response = await httpClient.PostAsync("api/items", content);

            if (!response.IsSuccessStatusCode)
            {
                using var contentStream = await response.Content.ReadAsStreamAsync();

                var message = await JsonSerializer.DeserializeAsync<ErrorViewModel>(contentStream);

                throw new WebApiCallException(message);
            }
            else
            {
                var json = await response.Content.ReadAsStringAsync();
                var responseContent = JsonSerializer.Deserialize<string>(json);
                return responseContent;
            }
        }

        public async Task DeleteItemAsync(string itemId)
        {
            var httpClient = _httpClientFactory.CreateClient("SimpleDataManagementSystemHttpClient");

            var response = await httpClient.DeleteAsync($"/api/items/{itemId}");

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

        public async Task<List<ItemViewModel>> GetAllItemsAsync(int? take = 8, int? page = 1)
        {
            var httpClient = _httpClientFactory.CreateClient("SimpleDataManagementSystemHttpClient");

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
                var responseContent = JsonSerializer.Deserialize<List<ItemViewModel>>(json);
                return responseContent;
            }
        }

        public async Task<ItemViewModel> GetItemByIdAsync(string itemId)
        {
            var httpClient = _httpClientFactory.CreateClient("SimpleDataManagementSystemHttpClient");

            var response = await httpClient.GetAsync($"/api/items/{itemId}");

            if (!response.IsSuccessStatusCode)
            {
                using var contentStream = await response.Content.ReadAsStreamAsync();

                var message = await JsonSerializer.DeserializeAsync<ErrorViewModel>(contentStream);

                throw new WebApiCallException(message);
            }
            else
            {
                var json = await response.Content.ReadAsStringAsync();
                var responseContent = JsonSerializer.Deserialize<ItemViewModel>(json);
                return responseContent;
            }
        }

        public async Task UpdateItemAsync(string itemId, UpdateItemViewModel updateItemViewModel)
        {
            var httpClient = _httpClientFactory.CreateClient("SimpleDataManagementSystemHttpClient");

            var content = new MultipartFormDataContent();

            if (updateItemViewModel.URLdoslike != null)
            {
                var fileContent = new StreamContent(updateItemViewModel.URLdoslike.OpenReadStream());
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(updateItemViewModel.URLdoslike.ContentType);

                content.Add(fileContent, "URLdoslike", updateItemViewModel.URLdoslike.FileName);
            }

            content.Add(new StringContent(updateItemViewModel.Nazivproizvoda, Encoding.UTF8, MediaTypeNames.Text.Plain), "nazivproizvoda");
            content.Add(new StringContent(updateItemViewModel.Datumakcije, Encoding.UTF8, MediaTypeNames.Text.Plain), "datumakcije");
            content.Add(new StringContent(updateItemViewModel.Cijena.ToString(), Encoding.UTF8, MediaTypeNames.Text.Plain), "cijena");
            content.Add(new StringContent(Convert.ToString(updateItemViewModel.Kategorija), Encoding.UTF8, MediaTypeNames.Text.Plain), "kategorija");
            content.Add(new StringContent(updateItemViewModel.Nazivretailera, Encoding.UTF8, MediaTypeNames.Text.Plain), "nazivretailera");
            content.Add(new StringContent(updateItemViewModel.Opis, Encoding.UTF8, MediaTypeNames.Text.Plain), "opis");

            var response = await httpClient.PutAsync($"api/items/{itemId}", content);

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
