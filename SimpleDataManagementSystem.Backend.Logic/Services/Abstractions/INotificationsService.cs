using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.Services.Abstractions
{
    public interface INotificationsService
    {
        public IItemUpdatedNotifierService ItemUpdatedNotifierService { get; }
    }

    public interface IItemUpdatedNotifierService
    {
        /// <summary>
        /// Sends notification message. 
        /// If <paramref name="excludedUsers"/> and <paramref name="sendToUsers"/> are empty, 
        ///     it then sends message to all clients listening.
        /// Either collection must be used. 
        ///     If both collections contain values, both will be ignored, and the message will be sent to all clients listening.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="excludedUsers"></param>
        /// <param name="sendToUsers"></param>
        /// <returns></returns>
        Task SendItemUpdatedNotificationAsync(
            string message,
            CancellationToken cancellationToken,
            List<int>? excludedUsers = null,
            List<int>? sendToUsers = null
        );
    }
}
