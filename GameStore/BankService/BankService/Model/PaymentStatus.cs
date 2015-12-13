using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankService.Model
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