using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shop.DataAccessLayer.Models
{
    public class StorageDirector
    {
        [Key]
        public int Id { get; set; }

        public string Surname { get; set; }

        public string Name { get; set; }

        public string Patronymic { get; set; }

        public DateTime Birthday { get; set; }

        public ICollection<Storage> Storages { get; set; }

        public StorageDirector()
        {
            Storages = new HashSet<Storage>();
        }
    }
}
