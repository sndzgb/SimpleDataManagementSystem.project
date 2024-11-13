using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.Options
{
    public sealed class AppOptions
    {
        /// <summary>
        /// Section name.
        /// </summary>
        public const string AppOptionsSectionName = "AppOptions";

        public string AdminEmail { get; set; }
    }
}
