using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Database.Entities
{
    [Table(name: "Categories", Schema = "dbo")]
    public class CategoryEntity
    {
        public CategoryEntity()
        {
            this.Items = new List<ItemEntity>();
        }


        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string Name { get; set; }

        public int Priority { get; set; }

        public ICollection<ItemEntity> Items { get; }
    }
}
