using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.Models.Entities
{
    public class CardInfo
    {
        public string Name { get; set; }

        public string CardNumber { get; set; }

        public string DateOfExpire { get; set; }

        public int Cvc { get; set; }

        public CardType CardType { get; set; }
    }
}
