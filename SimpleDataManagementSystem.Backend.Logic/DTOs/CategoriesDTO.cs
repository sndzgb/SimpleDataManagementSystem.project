using SimpleDataManagementSystem.Backend.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.DTOs
{
    public sealed class CategoriesDTO
    {
        public List<Category>? Categories { get; set; }
        public PageDTO PageInfo { get; set; }

        public class PageDTO
        {
            public int Total { get; set; }
        }
    }
}
