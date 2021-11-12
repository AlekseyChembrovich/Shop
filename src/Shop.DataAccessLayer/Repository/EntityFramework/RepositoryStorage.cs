using System.Collections.Generic;
using Shop.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Shop.DataAccessLayer.Repository.EntityFramework.Interfaces;

namespace Shop.DataAccessLayer.Repository.EntityFramework
{
    public class RepositoryStorage : BaseRepository<Storage>, IRepositoryStorage
    {
        public RepositoryStorage(DatabaseContext context) : base(context)
        {
        }

        public IEnumerable<Storage> GetAllIncludeForeignKey()
        {
            return Context.Storage
                .Include(x => x.StorageDirector);
        }
    }
}
