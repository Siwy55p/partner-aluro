using Microsoft.EntityFrameworkCore;
using partner_aluro.DAL;
using partner_aluro.Models;
using partner_aluro.Services.Interfaces;
using System.Xml.Linq;

namespace partner_aluro.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public int AddSave(Category category)
        {
            _context.Category.Add(category);
            var id = _context.SaveChanges();
            return id;
        }

        public int Delete(int id)
        {
            var category = _context.Category.Find(id);
            _context.Category.Remove(category);

            _context.SaveChanges();
            return id;
        }
        public int Delete(string name)
        {
            var category = _context.Category.Find(name);
            int id = category.CategoryId;
            _context.Category.Remove(category);

            _context.SaveChanges();
            return id;
        }

        public Category Get(int id)
        {
            var category = _context.Category.Find(id);

            return category;
        }
        public Category Get(string name)
        {
            var category = _context.Category.Find(name);

            return category;
        }

        public List<Category> List()
        {
            var category = _context.Category.ToList();

            return category;
        }

        public List<Category> List(string name)
        {
            var category = _context.Category.ToList();
            category.Find(a => a.Name == name);

            return category;
        }

        public List<Product> ListProductCategoryAll()
        {
            List<Product> produkty = _context.Products.ToList();
            return produkty;
        }

        public int Save(Category category)
        {
            var cat = _context.Update(category);
            _context.SaveChanges();

            return category.CategoryId;
        }

        public Category Update(Category category)
        {
            _context.Update(category);
            _context.SaveChanges();

            return category;
        }


    }
}
