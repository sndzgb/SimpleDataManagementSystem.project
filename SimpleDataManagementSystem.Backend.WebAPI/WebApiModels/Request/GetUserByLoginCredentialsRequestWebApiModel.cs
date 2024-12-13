namespace SimpleDataManagementSystem.Backend.WebAPI.WebApiModels.Request
{
    public sealed class GetUserByLoginCredentialsRequestWebApiModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
