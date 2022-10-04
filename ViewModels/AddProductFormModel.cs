using partner_aluro.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace partner_aluro.ViewModels
{

    public class AddProductFormModel
    {
        public List<Product> Products { get; set; }
        public Product Product { get; set; }
        public IList<SelectListItem> Categories { get; set; }

        public List<Category> CategoryList { get; set; }
    }
}
