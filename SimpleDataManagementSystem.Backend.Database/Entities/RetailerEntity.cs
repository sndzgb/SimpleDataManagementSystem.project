using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Database.Entities
{
    [Table(name: "Retailers", Schema = "dbo")]
    public class RetailerEntity
    {
        public RetailerEntity()
        {
            this.Items = new List<ItemEntity>();
        }


        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string Name { get; set; }
        
        public int Priority { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string? LogoImageUrl { get; set; }


        // new
        public virtual ICollection<ItemEntity> Items { get; set; }
    }
}
