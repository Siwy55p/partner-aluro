using Microsoft.AspNetCore.Mvc;
using partner_aluro.Services.Interfaces;

namespace partner_aluro.ViewComponents
{
    public class ProduktListViewComponent : ViewComponent
    {
        private readonly ICategoryService _categoryBD;
        private readonly IProductService _productService;
        public ProduktListViewComponent(ICategoryService category,IProductService productService)
        {
            _categoryBD = category;
            _productService = productService;
        }

        public IViewComponentResult Invoke()
        {

            //var result = _categoryBD.List();
            var produkty = _productService.GetProductList();

            return View(produkty);
        }
    }
}
