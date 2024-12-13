using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.DTOs.Request
{
    public class GetItemRequestDTO
    {
        public int RequestedByUserId { get; set; }
        
        public string ItemId { get; set; }
        public bool IncludeCategory { get; set; }
        public bool IncludeRetailer { get; set; }
        public MonitoredItemDTO Monitoring { get; set; }

        public class MonitoredItemDTO
        {
            public bool IncludeMonitoring { get; set; }
            //public int Take { get; set; }
            //public int Page { get; set; }
        }
    }
}
