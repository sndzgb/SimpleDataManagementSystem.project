using System.Text.Json.Serialization;

namespace SimpleDataManagementSystem.Backend.WebAPI.WebApiModels.Read
{
    public class ItemsWebApiModel
    {
        public ItemsWebApiModel()
        {
            this.Items = new List<ItemWebApiModel>();
            this.PageInfo = new PagedWebApiModel();
        }

        [JsonPropertyName("items")]
        public List<ItemWebApiModel> Items { get; set; }
        
        [JsonPropertyName("pageInfo")]
        public PagedWebApiModel PageInfo { get; set; }
    }
}
