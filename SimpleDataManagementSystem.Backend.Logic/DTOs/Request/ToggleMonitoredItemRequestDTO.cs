using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.DTOs.Request
{
    public class ToggleMonitoredItemRequestDTO
    {
        public ToggleMonitoredItemRequestDTO(
                ToggleMonitoredItemRequestMetadata toggleMonitoredItemRequestMetadata,
                int requestedByUserId
            )
        {
            ArgumentNullException.ThrowIfNull(nameof(toggleMonitoredItemRequestMetadata));
            ArgumentNullException.ThrowIfNull(nameof(requestedByUserId));

            this.RequestMetadata = toggleMonitoredItemRequestMetadata;
            this.RequestedByUserId = requestedByUserId;
        }

        public int RequestedByUserId { get; private set; }

        public ToggleMonitoredItemRequestMetadata RequestMetadata { get; private set; }

        public class ToggleMonitoredItemRequestMetadata
        {
            public ToggleMonitoredItemRequestMetadata(string itemId)
            {
                this.ItemId = itemId;
            }

            public string ItemId { get; private set; }
        }
    }
}
