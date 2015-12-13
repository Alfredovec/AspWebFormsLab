using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BankService.Model
{
    [DataContract]
    public class PaymentData
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string AccountNumber { get; set; }

        [DataMember]
        public string Cvv2 { get; set; }

        [DataMember]
        public decimal Amount { get; set; }

        [DataMember]
        public string Owner { get; set; }

        [DataMember]
        public int ExpirationMonth { get; set; }

        [DataMember]
        public int ExpirationYear { get; set; }

        public string ExpirationDate
        {
            get { return string.Format("{0}/{1}", ExpirationMonth, ExpirationYear); }
        }
    }
}