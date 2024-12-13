using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.DTOs.Request
{
    public sealed class ToggleItemEnabledDisabledStatusRequestDTO
    {
        public ToggleItemEnabledDisabledStatusRequestDTO(
                ToggleItemEnabledDisabledStatusRequestMetadata toggleItemEnabledDisabledStatusRequestMetadata
            )
        {
            this.Metadata = toggleItemEnabledDisabledStatusRequestMetadata;
        }

        public int RequestedByUserId { get; set; }

        public ToggleItemEnabledDisabledStatusRequestMetadata Metadata { get; set; }

        public class ToggleItemEnabledDisabledStatusRequestMetadata
        {
            public ToggleItemEnabledDisabledStatusRequestMetadata(string itemId)
            {
                this.ItemId = itemId;
            }

            public string ItemId { get; private set; }
        }
    }
}
