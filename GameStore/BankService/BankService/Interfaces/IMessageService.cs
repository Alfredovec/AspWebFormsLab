using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankService.Interfaces
{
    interface IMessageService
    {
        void SendSms(string phone, string text);
        
        void SendEmail(string email, string text);
    }
}
