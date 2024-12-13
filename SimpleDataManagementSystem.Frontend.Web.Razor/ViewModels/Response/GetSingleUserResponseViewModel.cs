using System.Text.Json.Serialization;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Response
{
	public sealed class GetSingleUserResponseViewModel
	{
		[JsonPropertyName("id")]
		public int Id { get; set; }

		[JsonPropertyName("username")]
		public string Username { get; set; }

		[JsonPropertyName("isPasswordChangeRequired")]
		public bool IsPasswordChangeRequired { get; set; }

		[JsonPropertyName("createdUTC")]
		public DateTime CreatedUTC { get; set; }

		[JsonPropertyName("roleName")]
		public string RoleName { get; set; }

		[JsonPropertyName("roleId")]
		public int RoleId { get; set; }
	}
}
