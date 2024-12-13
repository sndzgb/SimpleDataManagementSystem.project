namespace SimpleDataManagementSystem.Backend.WebAPI.WebApiModels.Response
{
    public sealed class GetSingleUserResponseWebApiModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public bool IsPasswordChangeRequired { get; set; }
        public DateTime CreatedUTC { get; set; }
        public string RoleName { get; set; }
        public int RoleId { get; set; }
    }
}
