using partner_aluro.Core.Repositories;
using partner_aluro.Models;
using partner_aluro.Services.Interfaces;

namespace partner_aluro.Services
{
    public class UnitOfWorkProduct : IUnitOfWorkProduct
    {
        public IProductService Product { get; }

        public ICategoryBD Category { get; }

        public UnitOfWorkProduct(IProductService product, ICategoryBD category)
        {
            Product = product;
            Category = category;
        }
    }
}
