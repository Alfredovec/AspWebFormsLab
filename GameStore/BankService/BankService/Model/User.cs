using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankService.Model
{
    public class User
    {
        public string Name { get; set; }
        
        public string Surname { get; set; }
        
        public string AccountNumber { get; set; }

        public string Email { get; set; }

        public decimal Money { get; set; }

        public string Phone { get; set; }

        public CardType CardType { get; set; }

        public string Cvv2 { get; set; }

        public string ExpirationDate { get; set; }
    }
}