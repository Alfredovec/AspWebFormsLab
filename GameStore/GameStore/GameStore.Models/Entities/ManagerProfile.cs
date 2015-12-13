using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.Models.Enums;

namespace GameStore.Models.Entities
{
    public class ManagerProfile
    {
        [ForeignKey("User")]
        public long Id { get; set; }

        public virtual User User{ get; set; }

        public NotifyStatus NotifyStatus { get; set; }
    }
}