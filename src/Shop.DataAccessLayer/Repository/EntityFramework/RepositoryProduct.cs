using System.Collections.Generic;
using Shop.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Shop.DataAccessLayer.Repository.EntityFramework.Interfaces;

namespace Shop.DataAccessLayer.Repository.EntityFramework
{
    public class RepositoryProduct : BaseRepository<Product>, IRepositoryProduct
    {
        public RepositoryProduct(DatabaseContext context) : base(context)
        {
        }

        public IEnumerable<Product> GetAllIncludeForeignKey()
        {
            return Context.Product
                .Include(x => x.Storage)
                .Include(x => x.TypeProduct);
        }
    }
}
