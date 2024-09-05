﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.Models
{
    public class Item
    {
        public string Nazivproizvoda { get; set; } = string.Empty;
        public string? Opis { get; set; }
        public string? Datumakcije { get; set; }
        public string? Nazivretailera { get; set; }

        public string? URLdoslike { get; set; }
        public decimal Cijena { get; set; }

        public Category? Category { get; set; }
        public Retailer Retailer { get; set; }
    }
}
