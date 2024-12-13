using System.Text.Json.Serialization;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Response
{
    public sealed class GetMultipleUsersResponseViewModel
	{
		[JsonPropertyName("users")]
		public List<UserViewModel> Users { get; set; }

		[JsonPropertyName("pageInfo")]
		public PagedViewModel PageInfo { get; set; }

		public class UserViewModel
		{
			[JsonPropertyName("id")]
			public int Id { get; set; }

			[JsonPropertyName("username")]
			public string Username { get; set; }
			
			[JsonPropertyName("isPasswordChangeRequired")]
			public bool IsPasswordChangeRequired { get; set; }
			
			[JsonPropertyName("createdUTC")]
			public DateTime CreatedUTC { get; set; }
			
			[JsonPropertyName("role")]
			public RoleViewModel Role { get; set; }

			public class RoleViewModel
			{
				[JsonPropertyName("id")]
				public int Id { get; set; }
				
				[JsonPropertyName("name")]
				public string Name { get; set; }
			}
		}

		//public class PageViewModel
		//{
		//	[JsonPropertyName("total")]
		//	public int Total { get; set; }
			
		//	[JsonPropertyName("page")]
		//	public int Page { get; set; }
			
		//	[JsonPropertyName("take")]
		//	public int Take { get; set; }
		//}
	}
}
