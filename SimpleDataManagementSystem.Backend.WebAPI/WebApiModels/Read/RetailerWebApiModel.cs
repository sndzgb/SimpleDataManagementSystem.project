namespace SimpleDataManagementSystem.Backend.WebAPI.WebApiModels.Read
{
    public class RetailerWebApiModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Priority { get; set; }
        public string? LogoImageUri { get; set; }
    }
}
