using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankService.Model;

namespace BankService.Interfaces
{
    interface ITransferRepository
    {
        void Add(Transfer transfer);

        IEnumerable<Transfer> Get();

        Transfer Get(string token);
    }
}
