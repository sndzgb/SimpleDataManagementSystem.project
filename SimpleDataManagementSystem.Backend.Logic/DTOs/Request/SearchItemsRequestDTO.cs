using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.DTOs.Request
{
    public sealed class SearchItemsRequestDTO
    {
		public int RequestedByUserId { get; set; }

		public bool IncludeCategory { get; set; }
        public bool IncludeRetailer { get; set; }

        public ItemMonitoringDTO Monitoring { get; set; }

        public PageDTO PageInfo { get; set; }

        public class PageDTO
        {
            public bool IsEnabled { get; set; }
            public string Query { get; set; }
            public int Take { get; set; }
            public int Page { get; set; }
            public SortableItem SortBy { get; set; }
        }

        public class ItemMonitoringDTO
        {
            public bool IncludeMonitoring { get; set; }
        }
    }
}
