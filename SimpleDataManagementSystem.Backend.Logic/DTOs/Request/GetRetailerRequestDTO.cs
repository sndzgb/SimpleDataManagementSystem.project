using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.DTOs.Request
{
    public sealed class GetRetailerRequestDTO
    {
        public int RequestedByUserId { get; set; }

        public int RetailerId { get; set; }
    }
}
