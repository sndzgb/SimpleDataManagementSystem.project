using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.DTOs.Request
{
    public sealed class UpdatePasswordRequestDTO
    {
        public UpdatePasswordRequestDTO(UpdatePasswordRequestMetadata updatePasswordRequestMetadata)
        {
            ArgumentNullException.ThrowIfNull(nameof(updatePasswordRequestMetadata));

            this.RequestMetadata = updatePasswordRequestMetadata;
        }

        public UpdatePasswordRequestMetadata RequestMetadata { get; set; }

        public string OldPassword { get; set; }
        public string NewPassword { get; set; }

        public class UpdatePasswordRequestMetadata
        {
            public UpdatePasswordRequestMetadata(int userId)
            {
                this.UserId = userId;
            }

            public int UserId { get; private set; }
        }
    }
}
