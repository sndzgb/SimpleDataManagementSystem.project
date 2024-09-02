using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SimpleDataManagementSystem.Backend.Logic.Models
{
    public class Retailer
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Priority { get; set; }
        public string LogoImageUrl { get; set; }
    }
}
