using Microsoft.AspNetCore.Mvc;
using partner_aluro.Services.Interfaces;

namespace partner_aluro.ViewComponents
{
    public class MenuMainViewComponent : ViewComponent
    {
        private readonly ICategoryService _categoryBD;
        public MenuMainViewComponent(ICategoryService category)
        {
            _categoryBD = category;
        }

        public IViewComponentResult Invoke()
        {
            var resul = _categoryBD.List();
            return View(resul);
        }
    }
}
