using partner_aluro.Models;

namespace partner_aluro.Services.Interfaces
{
    public interface IProductService
    {
        int AddProduct(Product product);
        List<Product> GetProductList();

        Product GetProductId(int id);

        Category GetCategoryId(int id);

        Category GetCategoryName(string name);
        int GetIdCategoryForName(string name);

        List<Category> GetListCategory();

        ICollection<Category> GetCategory();

        Product UpdateProduct(Product produkt);

        int DeleteProductId(int id);
    }
}
