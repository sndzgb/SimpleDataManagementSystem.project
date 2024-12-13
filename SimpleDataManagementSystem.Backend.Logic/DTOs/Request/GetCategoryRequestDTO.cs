using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.DTOs.Request
{
    // TODO base DTO: RequestedByUserId {get;set;}
    public sealed class GetCategoryRequestDTO
    {
        public int RequestedByUserId { get; set; }

        public int CategoryId { get; set; }
    }
}
