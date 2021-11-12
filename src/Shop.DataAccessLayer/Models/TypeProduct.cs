using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shop.DataAccessLayer.Models
{
    public class TypeProduct
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public double ExtraCharge { get; set; }

        public ICollection<Product> Products { get; set; }

        public TypeProduct()
        {
            Products = new HashSet<Product>();
        }
    }
}
