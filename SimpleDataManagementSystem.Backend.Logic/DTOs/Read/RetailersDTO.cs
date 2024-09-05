using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.DTOs.Read
{
    public class RetailersDTO
    {
        public RetailersDTO()
        {
            this.Retailers = new List<RetailerDTO>();
            this.PageInfo = new PagedDTO();
        }


        public List<RetailerDTO> Retailers { get; set; }
        public PagedDTO PageInfo { get; set; }
    }
}
