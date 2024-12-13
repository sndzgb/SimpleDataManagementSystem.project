using SimpleDataManagementSystem.Backend.Logic.DTOs.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.DTOs.Response
{
    public sealed class GetRetailersResponseDTO
    {
        public List<RetailerDTO>? Retailers { get; set; }
        public PageDTO PageInfo { get; set; }

        public class RetailerDTO
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string LogoImageUrl { get; set; }
            public int Priority { get; set; }
        }

        public class PageDTO 
        {
            public int Total { get; set; }
        }
    }
}
