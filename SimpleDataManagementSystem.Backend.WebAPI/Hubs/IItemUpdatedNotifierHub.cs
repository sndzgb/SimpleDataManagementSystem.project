namespace SimpleDataManagementSystem.Backend.WebAPI.Hubs
{
    public interface IItemUpdatedNotifierHub
    {
        Task ItemUpdatedNotifierSender(string message);
    }
}
