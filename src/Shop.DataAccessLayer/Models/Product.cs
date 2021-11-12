using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shop.DataAccessLayer.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public int Balance { get; set; }

        public decimal Price { get; set; }

        public int MinValue { get; set; }

        public int TypeProductId { get; set; }

        public int StorageId { get; set; }

        public virtual TypeProduct TypeProduct { get; set; }

        public virtual Storage Storage { get; set; }

        public ICollection<ProvideProduct> ProvidesProduct { get; set; }

        public Product()
        {
            ProvidesProduct = new HashSet<ProvideProduct>();
        }
    }
}
