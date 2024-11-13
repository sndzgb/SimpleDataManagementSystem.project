using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.Options
{
    public class EmailClientOptions
    {
        /// <summary>
        /// Section name.
        /// </summary>
        public const string EmailOptionsSectionName = "EmailClientOptions";

        public string Host { get; set; }
        public int Port { get; set; }

        public string NoReplyEmail { get; set; }
        public NetworkCredentials? Credentials { get; set; }

        public class NetworkCredentials
        {
            public string? Username { get; set; }
            public string? Password { get; set; }
        }
    }
}
