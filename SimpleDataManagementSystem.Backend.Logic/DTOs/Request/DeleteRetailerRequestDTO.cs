using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.DTOs.Request
{
    public sealed class DeleteRetailerRequestDTO
    {
        public DeleteRetailerRequestDTO(DeleteRetailerRequestMetadata deleteRetailerRequestMetadata)
        {
            this.Metadata = deleteRetailerRequestMetadata;
        }

        public int RequestedByUserId { get; set; }

        public DeleteRetailerRequestMetadata Metadata { get; set; }

        public class DeleteRetailerRequestMetadata
        {
            public DeleteRetailerRequestMetadata(int retailerId)
            {
                this.RetailerId = retailerId;
            }

            public int RetailerId { get; private set; }
        }
    }
}
