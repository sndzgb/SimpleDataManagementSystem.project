using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.DTOs.Request
{
    public sealed class CreateItemRequestDTO
    {
        public int RequestedByUserId { get; set; }

        public string Nazivproizvoda { get; set; }
        public string Opis { get; set; }
        public decimal Cijena { get; set; }
        public string URLdoslike { get; set; }
        public bool IsEnabled { get; set; }
        public string Datumakcije { get; set; }
        public int Kategorija { get; set; }
        public int RetailerId { get; set; }
    }
}
