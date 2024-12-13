using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.DTOs.Request
{
    public sealed class UpdateItemRequestDTO
    {
        public UpdateItemRequestDTO(UpdateItemRequestMetadata requestMetadata)
        {
            ArgumentNullException.ThrowIfNull(requestMetadata, nameof(requestMetadata));

            this.RequestMetadata = requestMetadata;
        }

        public int RequestedByUserId { get; set; }

        public int CategoryId { get; set; }
        public decimal Cijena { get; set; }
        public int RetailerId { get; set; }
        public string Datumakcije { get; set; }
        public string? Opis { get; set; }
        public string? URLdoslike { get; set; }
        public bool IsEnabled { get; set; }

        public bool DeleteCurrentURLdoslike { get; set; }
        public bool IsMonitoredByUser { get; set; }

        public UpdateItemRequestMetadata RequestMetadata { get; private set; }

        public class UpdateItemRequestMetadata
        {
            public UpdateItemRequestMetadata(string itemId)
            {
                this.ItemId = itemId;
            }

            public string ItemId { get; private set; }
        }
    }
}
