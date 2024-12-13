using SimpleDataManagementSystem.Backend.WebAPI.Constants;

namespace SimpleDataManagementSystem.Backend.WebAPI.Services.Abstractions
{
    public interface IUserConnectionTrackerService
    {
        void Add(Connection connection);
        void Remove(Connection connection);
        Connection Get(int userId, HubService hub);
    }

    public class Connection
    {
        public int UserId { get; set; }
        public string ConnectionId { get; set; }
        public HubService Hub { get; set; }
    }
}
