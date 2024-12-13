﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.Models
{
    public class Category
    {
        public int ID { get; set; }

        public string Name { get; set; } = string.Empty;

        public int Priority { get; set; }

        public ICollection<Item>? Items { get; set; }
    }
}
