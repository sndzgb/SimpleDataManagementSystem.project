using System.Text.Json.Serialization;

namespace SimpleDataManagementSystem.Backend.WebAPI.WebApiModels.Response
{
    public sealed class SearchItemsResponseWebApiModel
    {
        public List<ItemWebApiModel>? Items { get; set; }
        public PageWebApiModel PageInfo { get; set; }

        // TODO extract
        public class PageWebApiModel
        {
            public int Total { get; set; }
            public int Page { get; set; }
            public int Take { get; set; }
        }

        public class ItemWebApiModel
        {
            public string Nazivproizvoda { get; set; }
            public string Opis { get; set; }
            public string Datumakcije { get; set; }

            [JsonPropertyName("URLdoslike")]
            public string URLdoslike { get; set; }
            public decimal Cijena { get; set; }
            public bool IsEnabled { get; set; }
            public bool IsMonitoredByCurrentUser { get; set; }
            public int TotalUsersMonitoringThisItem { get; set; }

            public RetailerWebApiModel Retailer { get; set; }
            public CategoryWebApiModel Category { get; set; }

            //public IEnumerable<MonitoredItemDTO> MonitoredBy { get; set; }

            public class RetailerWebApiModel
            {
                public int Id { get; set; }
                public string Name { get; set; }
                public int Priority { get; set; }
            }

            public class CategoryWebApiModel
            {
                public int Id { get; set; }
                public string Name { get; set; }
            }
        }
    }
}
