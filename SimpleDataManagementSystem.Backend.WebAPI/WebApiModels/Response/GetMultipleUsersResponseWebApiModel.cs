namespace SimpleDataManagementSystem.Backend.WebAPI.WebApiModels.Response
{
    public sealed class GetMultipleUsersResponseWebApiModel
    {
        public List<UserWebApiModel> Users { get; set; }
        public PageWebApiModel PageInfo { get; set; }

        public class UserWebApiModel
        {
            public int Id { get; set; }
            public string Username { get; set; }
            public bool IsPasswordChangeRequired { get; set; }
            public DateTime CreatedUTC { get; set; }
            public RoleWebApiModel Role { get; set; }

            public class RoleWebApiModel
            {
                public int Id { get; set; }
                public string Name { get; set; }
            }
        }

        public class PageWebApiModel
        {
            public int Total { get; set; }
            public int Page { get; set; }
            public int Take { get; set; }
        }
    }
}
