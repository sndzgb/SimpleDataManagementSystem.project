using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.Models
{
    public sealed class MonitoredItem
    {
        public User User { get; set; }

        public Item Item { get; set; }

        public DateTime StartedMonitoringAtUtc { get; set; }
    }
}
