using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SimpleDataManagementSystem.Frontend.Web.Razor.Pages.ViewComponents
{
    public class InvalidModelStateViewComponent : ViewComponent
    {
        public ModelStateDictionary? ModelStateDictionary { get; set; }


        public IViewComponentResult Invoke(ModelStateDictionary? modelStateDictionary)
        {
            ModelStateDictionary = modelStateDictionary;

            return View(model: ModelStateDictionary);
        }
    }
}
