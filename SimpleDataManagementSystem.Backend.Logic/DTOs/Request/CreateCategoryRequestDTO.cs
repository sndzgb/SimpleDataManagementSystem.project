using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.DTOs.Request
{
    public sealed class CreateCategoryRequestDTO
    {
        public string Name { get; set; }
        public int Priority { get; set; }
        public int RequestedByUserId { get; set; }
    }
}
