using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankService.Model
{
    class Transfer
    {
        public long Id { get; set; }
        
        public PaymentStatus Status { get; set; }
        
        public DateTime Date { get; set; }

        public User User { get; set; }

        public decimal Amount { get; set; }

        public string ConfirmationCode { get; set; }
    }
}