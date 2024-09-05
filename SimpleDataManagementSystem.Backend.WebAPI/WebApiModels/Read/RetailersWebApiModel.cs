using System.Text.Json.Serialization;

namespace SimpleDataManagementSystem.Backend.WebAPI.WebApiModels.Read
{
    public class RetailersWebApiModel
    {
        public RetailersWebApiModel()
        {
            this.Retailers = new List<RetailerWebApiModel>();
            this.PageInfo = new PagedWebApiModel();
        }

        [JsonPropertyName("retailers")]
        public List<RetailerWebApiModel> Retailers { get; set; }
        
        [JsonPropertyName("pageInfo")]
        public PagedWebApiModel PageInfo { get; set; }
    }
}
