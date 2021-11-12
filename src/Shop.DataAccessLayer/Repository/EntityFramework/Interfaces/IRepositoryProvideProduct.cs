using System.Collections.Generic;
using Shop.DataAccessLayer.Models;

namespace Shop.DataAccessLayer.Repository.EntityFramework.Interfaces
{
    public interface IRepositoryProvideProduct : IRepository<ProvideProduct>
    {
        IEnumerable<ProvideProduct> GetAllIncludeForeignKey();
    }
}
