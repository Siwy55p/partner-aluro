using Microsoft.AspNetCore.Mvc;
using partner_aluro.Services.Interfaces;

namespace partner_aluro.ViewComponents
{
    public class MenuCategoryViewComponent : ViewComponent
    {
        private readonly ICategoryBD _categoryBD;
        public MenuCategoryViewComponent(ICategoryBD category)
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
