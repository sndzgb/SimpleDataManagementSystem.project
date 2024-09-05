using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.DTOs.Read
{
    public class UsersDTO
    {
        public UsersDTO()
        {
            this.Users = new List<UserDTO>();
            this.PageInfo = new PagedDTO();
        }


        public List<UserDTO> Users { get; set; }
        public PagedDTO PageInfo { get; set; }
    }
}
