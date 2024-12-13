namespace SimpleDataManagementSystem.Backend.WebAPI.WebApiModels.Response
{
    public sealed class GetRolesWebApiModel
    {
        public List<RoleWebApiModel> Roles { get; set; }
        public PageWebApiModel PageInfo { get; set; }

        public class RoleWebApiModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class PageWebApiModel
        {
            public int Total { get; set; }
            public int Page { get; set; }
            public int Take { get; set; }
        }
    }
}
