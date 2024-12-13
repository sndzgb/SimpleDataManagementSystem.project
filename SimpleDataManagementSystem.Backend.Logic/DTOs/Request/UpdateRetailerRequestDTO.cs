using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.DTOs.Request
{
    public sealed class UpdateRetailerRequestDTO
    {
        public UpdateRetailerRequestDTO(UpdateRetailerRequestMetadata updateRetailerRequestMetadata)
        {
            this.RequestMetadata = updateRetailerRequestMetadata;
        }

        public UpdateRetailerRequestMetadata RequestMetadata { get; set; }

        public class UpdateRetailerRequestMetadata
        {
            public UpdateRetailerRequestMetadata(int retailerId)
            {
                this.RetailerId = retailerId;
            }

            public int RetailerId { get; private set; }
        }

        public bool DeleteCurrentLogoImage { get; set; }
        public int RequestedByUserId { get; set; }
        public string Name { get; set; }
        public int Priority { get; set; }
        public string LogoImageUrl { get; set; }
    }
}
