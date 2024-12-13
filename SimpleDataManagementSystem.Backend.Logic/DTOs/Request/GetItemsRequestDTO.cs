using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.DTOs.Request
{
    public class GetItemsRequestDTO
    {
        public int RequestedByUserId { get; set; }

        public bool EnabledOnly { get; set; }
        public bool IncludeCategory { get; set; }
        public bool IncludeRetailer { get; set; }

        public MonitoredItemDTO Monitoring { get; set; }
        public PageDTO PageInfo { get; set; }

        public class PageDTO
        {
            public int Take { get; set; } = 8;
            public int Page { get; set; } = 1;
            public SortableItem SortBy { get; set; }
        }

        public class MonitoredItemDTO
        {
            public bool IncludeMonitoring { get; set; }
            //public int Take { get; set; }
            //public int Page { get; set; }
        }
    }
}
