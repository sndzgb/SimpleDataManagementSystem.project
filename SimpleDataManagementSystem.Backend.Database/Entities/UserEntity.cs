using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Database.Entities
{
    public class UserEntity
    {
        public int? RoleId { get; set; }
        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public virtual RoleEntity Role { get; set; }
        public int RoleId { get; set; }
    }
}
