using Microsoft.AspNetCore.SignalR;
using SimpleDataManagementSystem.Backend.Logic.Services.Abstractions;
using SimpleDataManagementSystem.Backend.WebAPI.Constants;
using SimpleDataManagementSystem.Backend.WebAPI.Hubs;
using SimpleDataManagementSystem.Backend.WebAPI.Services.Abstractions;

namespace SimpleDataManagementSystem.Backend.WebAPI.Services.Implementations
{
    public class NotificationsService : INotificationsService
    {
        public NotificationsService(IItemUpdatedNotifierService itemUpdatedNotifierService)
        {
            ItemUpdatedNotifierService = itemUpdatedNotifierService;
        }


        public IItemUpdatedNotifierService ItemUpdatedNotifierService { get; private set; }
    }


    public class ItemUpdatedNotifierService : IItemUpdatedNotifierService
    {
        private readonly IHubContext<ItemUpdatedNotifierHub, IItemUpdatedNotifierHub> _itemUpdatedNotifierHubContext;
        private readonly IUserConnectionTrackerService _userConnectionTrackerService;


        public ItemUpdatedNotifierService(
                IHubContext<ItemUpdatedNotifierHub, IItemUpdatedNotifierHub> itemUpdatedNotifierHubContext,
                IUserConnectionTrackerService userConnectionTrackerService
            )
        {
            _itemUpdatedNotifierHubContext = itemUpdatedNotifierHubContext;
            _userConnectionTrackerService = userConnectionTrackerService;
        }


        public async Task SendItemUpdatedNotificationAsync(
                string message, 
                CancellationToken cancellationToken,
                List<int>? excludedUsers = null, 
                List<int>? sendToUsers = null
            )
        {
            if (
                    (excludedUsers != null && excludedUsers.Count > 0)
                    &&
                    (sendToUsers != null && sendToUsers.Count > 0)
                )
            {
                await _itemUpdatedNotifierHubContext
                        .Clients
                        .All
                        .ItemUpdatedNotifierSender(message);
                return;
            }


            if (excludedUsers != null && excludedUsers.Count > 0)
            {
                var excludedUsersConnectionIds = new List<string>();
                foreach (var eu in excludedUsers)
                {
                    var userConnection = _userConnectionTrackerService.Get(eu, HubService.ItemUpdatedNotifier);
                    excludedUsersConnectionIds.Add(userConnection.ConnectionId);
                }
                await _itemUpdatedNotifierHubContext
                        .Clients
                        .AllExcept(excludedUsersConnectionIds)
                        .ItemUpdatedNotifierSender(message);
            }


            if (sendToUsers != null && sendToUsers.Count > 0)
            {
                var sendToUsersConnectionIds = new List<string>();
                foreach (var eu in sendToUsers)
                {
                    var userConnection = _userConnectionTrackerService.Get(eu, HubService.ItemUpdatedNotifier);
                    sendToUsersConnectionIds.Add(userConnection.ConnectionId);
                }
                await _itemUpdatedNotifierHubContext
                        .Clients
                        .Clients(sendToUsersConnectionIds)
                        .ItemUpdatedNotifierSender(message);
            }
        }
    }
}
