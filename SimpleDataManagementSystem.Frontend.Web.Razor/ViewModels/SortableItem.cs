using System.ComponentModel;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.ViewModels
{
    public enum SortableItem
    {
        [Description("Naziv proizvoda asc")]
        NazivproizvodaAsc = 10,
        [Description("Naziv proizvoda desc")]
        NazivproizvodaDesc = 11,
        [Description("Cijena asc")]
        CijenaAsc = 20,
        [Description("Cijena desc")]
        CijenaDesc = 21
    }
}
