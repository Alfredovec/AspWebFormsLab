using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace GameStore.Models.Entities
{
    public class Role
    {
        public long Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        public virtual ICollection<User> Users { get; set; } 
    }
}
