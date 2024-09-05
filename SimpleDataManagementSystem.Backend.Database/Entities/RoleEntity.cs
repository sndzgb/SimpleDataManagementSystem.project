using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Database.Entities
{
    public class RoleEntity
    {
        public RoleEntity()
        {
            this.Users = new List<UserEntity>();
        }


        public DateTime CreatedUTC { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<UserEntity> Users { get; set; }
    }
}
