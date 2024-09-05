using SimpleDataManagementSystem.Backend.Logic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.DTOs.Read
{
    public class ItemDTO
    {
        public string Nazivproizvoda { get; set; }
        public string? Opis { get; set; }
        public string? Datumakcije { get; set; }
        public string? Nazivretailera { get; set; } // TODO maknuti -- višak
        public int RetailerId { get; set; }
        public string? URLdoslike { get; set; }
        public decimal Cijena { get; set; }
        public int? Kategorija { get; set; }

        //public ItemDTO(string nazivproizvoda, string opis, RetailerDTO retailer, CategoryDTO category)
        //{
        //    this.Nazivproizvoda = id;
        //    this.Opis = opis;
        //    this.Retailer = retailer;
        //    this.Category = category;
        //}

        //public RetailerDTO Retailer { get; set; }
        //public CategoryDTO Category { get; set; }
    }
}
