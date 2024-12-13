using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.DTOs.Request
{
    public sealed class DeleteCategoryRequestDTO
    {
        public DeleteCategoryRequestDTO(DeleteCategoryRequestMetadata deleteCategoryRequestMetadata)
        {
            this.Metadata = deleteCategoryRequestMetadata;
        }

        public DeleteCategoryRequestMetadata Metadata { get; set; }
        public int RequestedByUserId { get; set; }

        public class DeleteCategoryRequestMetadata
        {
            public DeleteCategoryRequestMetadata(int categoryId)
            {
                this.CategoryId = categoryId;
            }

            public int CategoryId { get; private set; }
        }
    }
}
