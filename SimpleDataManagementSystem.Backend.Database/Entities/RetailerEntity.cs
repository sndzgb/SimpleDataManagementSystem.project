using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Database.Entities
{
    public class RetailerEntity
    {
        public RetailerEntity()
        {
            this.Items = new List<ItemEntity>();
        }


        public int ID { get; set; }

        public string Name { get; set; }
        
        public int Priority { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string? LogoImageUrl { get; set; }

        public virtual ICollection<ItemEntity> Items { get; set; }
    }
}
