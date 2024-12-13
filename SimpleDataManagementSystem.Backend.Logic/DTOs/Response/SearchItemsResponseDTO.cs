using SimpleDataManagementSystem.Backend.Logic.DTOs.Request;
using SimpleDataManagementSystem.Backend.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.DTOs.Response
{
    public sealed class SearchItemsResponseDTO
    {
        public List<ItemDTO>? Items { get; set; }
        public PageDTO PageInfo { get; set; }

        public class PageDTO
        {
            public int Total { get; set; }
        }

        public class ItemDTO
        {
            public string Nazivproizvoda { get; set; }
            public string Opis { get; set; }
            public string Datumakcije { get; set; }
            public string URLdoslike { get; set; }
            public decimal Cijena { get; set; }
            public bool IsEnabled { get; set; }
            public bool IsMonitoredByCurrentUser { get; set; }
            public int TotalUsersMonitoringThisItem { get; set; }

            public RetailerDTO Retailer { get; set; }
            public CategoryDTO Category { get; set; }

            public class RetailerDTO
            {
                public int Id { get; set; }
                public string Name { get; set; }
                public int Priority { get; set; }
            }

            public class CategoryDTO
            {
                public int Id { get; set; }
                public string Name { get; set; }
            }
        }
    }
}
