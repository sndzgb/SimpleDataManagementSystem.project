﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.DTOs.Read
{
    public class RetailerDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Priority { get; set; }
        public string? LogoImageUrl { get; set; }
    }
}
