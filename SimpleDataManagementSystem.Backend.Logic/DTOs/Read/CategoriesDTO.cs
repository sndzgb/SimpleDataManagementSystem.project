using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.DTOs.Read
{
    public class CategoriesDTO
    {
        public CategoriesDTO()
        {
            this.Categories = new List<CategoryDTO>();
            this.PageInfo = new PagedDTO();
        }


        public List<CategoryDTO> Categories { get; set; }
        public PagedDTO PageInfo { get; set; }
    }
}
