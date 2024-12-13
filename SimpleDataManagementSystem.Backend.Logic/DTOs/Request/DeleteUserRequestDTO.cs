using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.DTOs.Request
{
    public sealed class DeleteUserRequestDTO
    {
        public DeleteUserRequestDTO(DeleteUserRequestMetadata deleteUserRequestMetadata)
        {
            this.Metadata = deleteUserRequestMetadata;
        }

        public DeleteUserRequestMetadata Metadata { get; set; }

        public int RequestedByUserId { get; set; }

        public class DeleteUserRequestMetadata
        {
            public DeleteUserRequestMetadata(int userId)
            {
                this.UserId = userId;
            }

            public int UserId { get; private set; }
        }
    }
}
