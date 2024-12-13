using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.DTOs.Response
{
    public sealed class GetCategoriesResponseDTO
    {
        public List<CategoryDTO> Categories { get; set; }
        public PageDTO PageInfo { get; set; }

        public class PageDTO
        {
            public int Total { get; set; }
            //public int Page { get; set; }
            //public int Take { get; set; }
        }

        public class CategoryDTO 
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Priority { get; set; }
        }
    }
}
