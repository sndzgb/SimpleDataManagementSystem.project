using SimpleDataManagementSystem.Shared.Common.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.DTOs.Request
{
    public sealed class UpdateUserRequestDTO
    {
        public UpdateUserRequestDTO(UpdateUserRequestMetadata requestMetadata)
        {
            ArgumentNullException.ThrowIfNull(requestMetadata, nameof(requestMetadata));

            this.RequestMetadata = requestMetadata;
        }

        public int RequestedByUserId { get; set; }
        public string Username { get; set; }
        public Roles Role { get; set; }

        public UpdateUserRequestMetadata RequestMetadata { get; private set; }

        public class UpdateUserRequestMetadata
        {
            public UpdateUserRequestMetadata(int userId)
            {
                this.UserId = userId;
            }

            public int UserId { get; private set; }
        }
    }
}
