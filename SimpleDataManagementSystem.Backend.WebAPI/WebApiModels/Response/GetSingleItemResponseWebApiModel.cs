using System.Text.Json.Serialization;

namespace SimpleDataManagementSystem.Backend.WebAPI.WebApiModels.Response
{
    public sealed class GetSingleItemResponseWebApiModel
    {
        public ItemWebApiModel? Item { get; set; }
        public MonitoredItemWebApiModel? Monitoring { get; set; }

        public class MonitoredItemWebApiModel
        {
            public int TotalUsersMonitoringThisItem { get; set; }
            public bool IsMonitoredByUser { get; set; }
            public DateTime? StartedMonitoringAtUtc { get; set; }
        }

        public class ItemWebApiModel
        {
            public string Nazivproizvoda { get; set; }
            public decimal Cijena { get; set; }
            public bool IsEnabled { get; set; }
            public string? Opis { get; set; }
            public string Datumakcije { get; set; }

            [JsonPropertyName("URLdoslike")]
            public string URLdoslike { get; set; }

            public RetailerWebApiModel Retailer { get; set; }
            public CategoryWebApiModel Category { get; set; }

            public class RetailerWebApiModel
            {
                public int Id { get; set; }
                public string Name { get; set; }
                public int Priority { get; set; }
                public string LogoImageUrl { get; set; }
            }

            public class CategoryWebApiModel
            {
                public int Id { get; set; }
                public string Name { get; set; }
                public int Priority { get; set; }
            }
        }
    }
}
