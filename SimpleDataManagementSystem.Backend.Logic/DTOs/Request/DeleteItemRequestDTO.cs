using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.DTOs.Request
{
    public sealed class DeleteItemRequestDTO
    {
        public DeleteItemRequestDTO(DeleteItemRequestMetadata deleteItemRequestMetadata)
        {
            this.Metadata = deleteItemRequestMetadata;
        }

        public DeleteItemRequestMetadata Metadata { get; set; }
        public int RequestedByUserId { get; set; }

        public class DeleteItemRequestMetadata
        {
            public DeleteItemRequestMetadata(string itemId)
            {
                this.ItemId = itemId;
            }

            public string ItemId { get; private set; }
        }
    }
}
