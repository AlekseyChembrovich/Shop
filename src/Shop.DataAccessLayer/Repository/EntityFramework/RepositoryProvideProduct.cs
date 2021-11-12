using System.Collections.Generic;
using Shop.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Shop.DataAccessLayer.Repository.EntityFramework.Interfaces;

namespace Shop.DataAccessLayer.Repository.EntityFramework
{
    public class RepositoryProvideProduct : BaseRepository<ProvideProduct>, IRepositoryProvideProduct
    {
        public RepositoryProvideProduct(DatabaseContext context) : base(context)
        {
        }

        public IEnumerable<ProvideProduct> GetAllIncludeForeignKey()
        {
            return Context.ProvideProduct
                .Include(x => x.Product)
                .Include(x => x.Order);
        }
    }
}
