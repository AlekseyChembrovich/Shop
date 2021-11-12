using Shop.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace Shop.DataAccessLayer.Repository.EntityFramework
{
    public class DatabaseContext : DbContext
    {
        public virtual DbSet<Client> Client { get; set; }

        public virtual DbSet<Driver> Driver { get; set; }

        public virtual DbSet<Order> Order { get; set; }

        public virtual DbSet<Product> Product { get; set; }

        public virtual DbSet<ProvideProduct> ProvideProduct { get; set; }

        public virtual DbSet<Provider> Provider { get; set; }

        public virtual DbSet<Storage> Storage { get; set; }

        public virtual DbSet<StorageDirector> StorageDirector { get; set; }

        public virtual DbSet<TypeProduct> TypeProduct { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-ALEKSEY\\SQLEXPRESS;Initial Catalog=Shop;Integrated Security=True");
            }
        }

        public DatabaseContext(DbContextOptions options) : base(options)
        {

        }

        public DatabaseContext() { }
    }
}
