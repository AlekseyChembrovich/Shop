using System.Collections.Generic;
using Shop.DataAccessLayer.Models;

namespace Shop.DataAccessLayer.Repository.EntityFramework.Interfaces
{
    public interface IRepositoryProduct : IRepository<Product>
    {
        IEnumerable<Product> GetAllIncludeForeignKey();
    }
}
