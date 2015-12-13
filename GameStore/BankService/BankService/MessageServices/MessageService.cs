using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BankService.Interfaces;

namespace BankService.MessageServices
{
    class MessageService : IMessageService
    {
        private readonly IEmailSender _emailSender;

        private readonly IPhoneSender _phoneSender;
        
        public MessageService(IEmailSender emailSender, IPhoneSender phoneSender)
        {
            _emailSender = emailSender;
            _phoneSender = phoneSender;
        }

        public void SendSms(string phone, string text)
        {
            _phoneSender.Send(phone,text);
        }

        public void SendEmail(string email, string text)
        {
            _emailSender.Send(email, text);
        }
    }
}