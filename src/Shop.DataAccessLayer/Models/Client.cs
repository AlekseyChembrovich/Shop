using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shop.DataAccessLayer.Models
{
    public class Client
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string TaxpayerIdentification { get; set; }

        public string Phone { get; set; }

        public ICollection<Order> Orders { get; set; }

        public Client()
        {
            Orders = new HashSet<Order>();
        }
    }
}
