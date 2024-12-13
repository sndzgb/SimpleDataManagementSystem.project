using System.Text.Json.Serialization;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels.Response
{
	public sealed class GetSingleRetailerResponseViewModel
	{
		[JsonPropertyName("id")]
		public int Id { get; set; }
		
		[JsonPropertyName("name")]
		public string Name { get; set; }
		
		[JsonPropertyName("priority")]
		public int Priority { get; set; }
		
		[JsonPropertyName("logoImageUrl")]
		public string LogoImageUrl { get; set; }
	}
}
