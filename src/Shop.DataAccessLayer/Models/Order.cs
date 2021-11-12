using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shop.DataAccessLayer.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public int Number { get; set; }

        public DateTime Date { get; set; }

        public int DriverId { get; set; }

        public int ProviderId { get; set; }

        public int ClientId { get; set; }

        public virtual Driver Driver { get; set; }

        public virtual Provider Provider { get; set; }

        public virtual Client Client { get; set; }

        public ICollection<ProvideProduct> ProvidesProduct { get; set; }

        public Order()
        {
            ProvidesProduct = new HashSet<ProvideProduct>();
        }
    }
}
