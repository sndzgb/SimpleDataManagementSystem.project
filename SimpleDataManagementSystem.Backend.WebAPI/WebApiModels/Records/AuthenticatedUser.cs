namespace SimpleDataManagementSystem.Backend.WebAPI.WebApiModels.Records
{
    public record AuthenticatedUser(int UserId, string Username, string[] Roles, bool IsPasswordChangeRequired);
}
