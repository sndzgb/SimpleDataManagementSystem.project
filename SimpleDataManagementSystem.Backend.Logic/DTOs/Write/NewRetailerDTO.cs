using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.DTOs.Write
{
    public class NewRetailerDTO
    {
        public string Name { get; set; }
        public int Priority { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string? LogoImageUrl { get; set; }
    }
}
