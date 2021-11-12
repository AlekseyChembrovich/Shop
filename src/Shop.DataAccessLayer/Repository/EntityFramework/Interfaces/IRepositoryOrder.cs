using System.Collections.Generic;
using Shop.DataAccessLayer.Models;

namespace Shop.DataAccessLayer.Repository.EntityFramework.Interfaces
{
    public interface IRepositoryOrder : IRepository<Order>
    {
        IEnumerable<Order> GetAllIncludeForeignKey();
    }
}
