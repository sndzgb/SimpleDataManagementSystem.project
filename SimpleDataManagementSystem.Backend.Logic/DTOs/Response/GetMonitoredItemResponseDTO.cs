using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.DTOs.Response
{
    public class GetMonitoredItemResponseDTO
    {
        public RequestDTO Request { get; set; }
        public ResponseDTO Response { get; set; }

        public class RequestDTO
        {
            public int UserId { get; set; }
            public string Nazivproizvoda { get; set; }
        }

        public class ResponseDTO 
        {
            public int UserId { get; set; }
            public string Nazivproizvoda { get; set; }
            public DateTime StartedMonitoringAtUtc { get; set; }
        }
    }
}
