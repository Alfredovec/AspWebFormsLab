using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Models.Entities
{
    public enum PaymentStatus
    {
        Confirmed,
        Created,
        NotEnoughMoney,
        NotFound,
        Error
    }
}
