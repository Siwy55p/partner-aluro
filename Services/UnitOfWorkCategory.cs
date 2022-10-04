using partner_aluro.Core.Repositories;
using partner_aluro.Models;
using partner_aluro.Services.Interfaces;

namespace partner_aluro.Services
{
    public class UnitOfWorkCategory : IUnitOfWorkCategory
    {
        public ICategoryBD Category { get; }


        public UnitOfWorkCategory(ICategoryBD category)
        {
            Category = category;
        }
    }
}
