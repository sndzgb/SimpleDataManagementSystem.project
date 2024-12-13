using SimpleDataManagementSystem.Backend.Logic.DTOs.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.DTOs.Response
{
    public sealed class GetItemResponseDTO
    {
        public string Nazivproizvoda { get; set; }
        public decimal Cijena { get; set; }
        public bool IsEnabled { get; set; }
        public string? Opis { get; set; }
        public string? Datumakcije { get; set; }
        public string? URLdoslike { get; set; }

        public MonitoredItemDTO? Monitoring { get; set; }
        public RetailerDTO Retailer { get; set; }
        public CategoryDTO Category { get; set; }

        public class RetailerDTO
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Priority { get; set; }
            public string? LogoImageUrl { get; set; }
        }

        public class CategoryDTO 
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Priority { get; set; }
        }

        public class MonitoredItemDTO
        {
            public bool IsMonitoredByUser { get; set; }
            public DateTime? StartedMonitoringAtUtc { get; set; }
            public int TotalUsersMonitoringThisItem { get; set; }
        }
    }
}
