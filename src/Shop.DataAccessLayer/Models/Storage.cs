using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shop.DataAccessLayer.Models
{
    public class Storage
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public int StorageDirectorId { get; set; }

        public virtual StorageDirector StorageDirector { get; set; }

        public ICollection<Product> Products { get; set; }

        public Storage()
        {
            Products = new HashSet<Product>();
        }
    }
}
