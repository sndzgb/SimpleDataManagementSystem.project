using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.DTOs.Request
{
    public sealed class GetCategoriesRequestDTO
    {
        public PageDTO PageInfo { get; set; }
        public int RequestedByUserId { get; set; }

        public class PageDTO
        {
            public int Page { get; set; }
            public int Take { get; set; }
        }
    }
}
