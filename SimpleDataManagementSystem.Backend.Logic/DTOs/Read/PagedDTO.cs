using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.DTOs.Read
{
    public class PagedDTO
    {
        public int Total { get; set; }
        public int Page { get; set; }
        public int Take { get; set; }
    }
}
