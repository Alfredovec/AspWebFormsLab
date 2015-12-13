using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GameStore.Web.Models
{
    public class ManagerProfileViewModel
    {
        public long Id { get; set; }

        [UIHint("Enum")]
        public NotifyStatusViewModel NotifyStatus { get; set; }
    }
}