using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.Models
{
    public class Role
    {
        public int ID { get; set; }
        public string Name { get; set; } = string.Empty;

        // TODO get all users for roleId
        public ICollection<User>? Users { get; set; }
    }
}
