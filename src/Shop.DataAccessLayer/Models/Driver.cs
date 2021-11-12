using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shop.DataAccessLayer.Models
{
    public class Driver
    {
        [Key]
        public int Id { get; set; }

        public string Surname { get; set; }

        public string Name { get; set; }

        public string Patronymic { get; set; }

        public int Experience { get; set; }

        public ICollection<Order> Orders { get; set; }

        public Driver()
        {
            Orders = new HashSet<Order>();
        }
    }
}
