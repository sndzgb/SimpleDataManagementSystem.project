namespace SimpleDataManagementSystem.Backend.WebAPI.WebApiModels.Response
{
    public sealed class GetSingleRetailerResponseWebApiModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Priority { get; set; }
        public string LogoImageUrl { get; set; }
    }
}
