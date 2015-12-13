using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BankService.Interfaces;

namespace BankService.MessageServices
{
    public class EmailSender : IEmailSender
    {
        public void Send(string email, string text)
        {
            return;
        }
    }
}