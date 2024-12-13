using System.Text.Json.Serialization;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Response
{
    public class GetAllRolesResponseViewModel
	{
		[JsonPropertyName("roles")]
		public List<RolesViewModel> Roles { get; set; }

		public class RolesViewModel
		{
			[JsonPropertyName("id")]
			public int Id { get; set; }

			[JsonPropertyName("name")]
			public string Name { get; set; }
		}

		[JsonPropertyName("pageInfo")]
		public PagedViewModel PageInfo { get; set; }

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
