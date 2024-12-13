using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.DTOs.Request
{
    public sealed class UpdateCategoryRequestDTO
    {
        public UpdateCategoryRequestDTO(UpdateCategoryRequestMetadata requestMetadata)
        {
            ArgumentNullException.ThrowIfNull(requestMetadata, nameof(requestMetadata));

            this.RequestMetadata = requestMetadata;
        }

        public int RequestedByUserId { get; set; }
        public int Priority { get; set; }
        public string Name { get; set; }

        public UpdateCategoryRequestMetadata RequestMetadata { get; private set; }

        public class UpdateCategoryRequestMetadata
        {
            public UpdateCategoryRequestMetadata(int categoryId)
            {
                this.CategoryId = categoryId;
            }

            public int CategoryId { get; private set; }
        }
    }
}
