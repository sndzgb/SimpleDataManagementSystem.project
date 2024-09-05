using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.DTOs.Read
{
    public class ItemsDTO
    {
        public ItemsDTO()
        {
            this.Items = new List<ItemDTO>();
            this.PageInfo = new PagedDTO();
        }


        public List<ItemDTO> Items { get; set; }
        public PagedDTO PageInfo { get; set; }
    }
}
