using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.DTOs.Write
{
    public class NewItemDTO
    {
        public string Nazivproizvoda { get; set; }
        public string Opis { get; set; }
        public string Datumakcije { get; set; }
        public string Nazivretailera { get; set; }
        public string URLdoslike { get; set; }
        public decimal Cijena { get; set; }
        public int Kategorija { get; set; }
    }
}
