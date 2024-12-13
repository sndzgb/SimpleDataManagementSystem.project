using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Database.Entities
{
	public class MonitoredItemEntity
	{
        public string ItemNazivproizvoda { get; set; }
        public ItemEntity Item { get; set; }

        public int UserId { get; set; }
        public UserEntity User { get; set; }

        public DateTime StartedMonitoringAtUtc { get; set; }
    }
}
