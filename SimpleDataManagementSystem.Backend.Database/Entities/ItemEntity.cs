using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Database.Entities
{
    public class ItemEntity
    {
        [Key]
        public string Nazivproizvoda { get; set; }
        public string? Opis { get; set; }
        public string Datumakcije { get; set; }
        public string Nazivretailera { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string? URLdoslike { get; set; }
        public decimal Cijena { get; set; }

        public int Kategorija { get; set; }
        public CategoryEntity Category { get; set; }

        public int RetailerID { get; set; }
        public RetailerEntity Retailer { get; set; }
    }
}
