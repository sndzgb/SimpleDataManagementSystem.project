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
    public class UserEntity
    {
        public int? RoleId { get; set; }
        public RoleEntity? Role { get; set; }

        public ICollection<MonitoredItemEntity> MonitoredItems { get; set; }


        public bool IsPasswordChangeRequired { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedUTC { get; set; }
        public int Id { get; set; }
        public string Username { get; set; }
    }
}
