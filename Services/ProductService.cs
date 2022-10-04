using partner_aluro.DAL;
using partner_aluro.Models;
using partner_aluro.Services.Interfaces;
using System.Runtime.Serialization;
using System.Security.Cryptography.Xml;

namespace partner_aluro.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public int DeleteProductId(int id)
        {
            var product = _context.Products.Find(id);
            _context.Products.Remove(product);
            _context.SaveChanges();
            return id;
        }

        public Product GetProductId(int id)
        {
            var product = _context.Products.Find(id);

            return product;
        }

        public List<Product> GetProductList()
        {
            var products = _context.Products.ToList();

            return products;
        }

        public int AddProduct(Product product)
        {
            _context.Products.Add(product);//Narazie wiemy co

            if(_context.SaveChanges() >0)
            {
                System.Console.WriteLine("Sukces");
            }; //tutaj jest dodanie - zwraca ilosc wykonanych operacji

            //logika zapisujaca do bazy
            return product.ProductId;
        }

        public int GetCategoryId(int id)
        {
            int category = _context.Category.Find(id).CategoryId;
            return category;
        }

        public List<Category> GetListCategory()
        {
            List<Category> listaCategori = _context.Category.ToList();
            return listaCategori;
        }

        public ICollection<Category> GetCategory()
        {
            return _context.Category.ToList();
        }

        public int GetCategoryName(string name)
        {
            return _context.Category.Find(name).CategoryId;
        }

        public int GetIdCategoryForName(string name)
        {
            int category = _context.Category.Find(name).CategoryId;
            return category;
        }


        Category IProductService.GetCategoryName(string name)
        {
            return _context.Category.Find(name);
        }

        public Product UpdateProduct(Product produkt)
        {

            _context.Update(produkt);
            _context.SaveChanges();

            return produkt;
        }
    }
}
