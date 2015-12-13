using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Models.Entities
{
    public class User
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public DateTime? ActivationTime { get; set; }

        public virtual ICollection<Role> Roles { get; set; }

        public virtual ManagerProfile ManagerProfile { get; set; }

        public virtual ICollection<Comment> Comments { get; set; } 
    }
}
