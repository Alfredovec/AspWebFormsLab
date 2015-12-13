using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankService.Model;

namespace BankService.Interfaces
{
    interface IUserRepository
    {
        IEnumerable<User> Get();

        User Get(string name, string accountNumber, CardType type, string cvv, string expirationDate);
    }
}
