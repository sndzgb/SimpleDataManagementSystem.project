namespace SimpleDataManagementSystem.Backend.WebAPI.WebApiModels.Response
{
    public sealed class CreateItemResponseWebApiModel
    {
        public string Nazivproizvoda { get; set; }
        public string? Opis { get; set; }
        public string? DatumAkcije { get; set; }
        public string? URLdoslike { get; set; }
        public decimal Cijena { get; set; }
        public bool IsEnabled { get; set; }

        public CategoryWebApiModel Category { get; set; }
        public RetailerWebApiModel Retailer { get; set; }

        public class CategoryWebApiModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Priority { get; set; }
        }

        public class RetailerWebApiModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Priority { get; set; }
            public string? LogoImageUrl { get; set; }
        }
    }
}
