using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankService.Interfaces
{
    interface IPhoneSender
    {
        void Send(string phone, string text);
    }
}
