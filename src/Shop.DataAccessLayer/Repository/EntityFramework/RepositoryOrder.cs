using System.Collections.Generic;
using Shop.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Shop.DataAccessLayer.Repository.EntityFramework.Interfaces;

namespace Shop.DataAccessLayer.Repository.EntityFramework
{
    public class RepositoryOrder : BaseRepository<Order>, IRepositoryOrder
    {
        public RepositoryOrder(DatabaseContext context) : base(context)
        {
        }

        public IEnumerable<Order> GetAllIncludeForeignKey()
        {
            return Context.Order
                .Include(x => x.Driver)
                .Include(x => x.Client)
                .Include(x => x.Provider);
        }
    }
}
