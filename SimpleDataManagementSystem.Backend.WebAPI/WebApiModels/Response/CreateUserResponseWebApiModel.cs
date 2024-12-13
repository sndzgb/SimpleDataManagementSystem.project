namespace SimpleDataManagementSystem.Backend.WebAPI.WebApiModels.Response
{
    public sealed class CreateUserResponseWebApiModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public int RoleId { get; set; }
        public bool IsPasswordChangeRequired { get; set; }
    }
}
