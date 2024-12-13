using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Shared.Options
{
    public sealed class CorsOptions
    {
        /// <summary>
        /// Section name.
        /// </summary>
        public const string CorsOptionsSectionName = "CorsOptions";


        public string[] AllowedOrigins { get; set; }
    }
}
