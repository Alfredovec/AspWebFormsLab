using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using BankService.Interfaces;
using BankService.MessageServices;
using BankService.Model;
using BankService.Repositories;

namespace BankService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "PaymentService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select PaymentService.svc or PaymentService.svc.cs at the Solution Explorer and start debugging.
    public class PaymentService : IPaymentService
    {
        private readonly IUserRepository _userRepository = new UserRepository();

        private readonly ITransferRepository _transferRepository = new TransferRepository();

        private readonly IMessageService _messageService = new MessageService(new EmailSender(), new PhoneSender());

        public PaymentStatus PayByVisa(PaymentData data)
        {
            return Pay(data, CardType.Visa);
        }

        public PaymentStatus PayByMasterCard(PaymentData data)
        {
            return Pay(data, CardType.MasterCard);
        }

        public PaymentStatus Confirm(string token)
        {
            var transfer = _transferRepository.Get(token);
            if (transfer == null)
            {
                return PaymentStatus.NotFound;
            }

            transfer.User.Money -= transfer.Amount;
            _messageService.SendEmail(transfer.User.Email, "Transfer sum: " + transfer.Amount);

            return PaymentStatus.Confirmed;
        }

        private PaymentStatus Pay(PaymentData data, CardType type)
        {
            var user = _userRepository.Get(data.Name, data.AccountNumber, type, data.Cvv2, data.ExpirationDate);
            if (user == null)
            {
                return PaymentStatus.NotFound;
            }

            if (user.Money < data.Amount)
            {
                return PaymentStatus.NotEnoughMoney;
            }

            var transfer = new Transfer
            {
                Date = DateTime.UtcNow,
                Amount =  data.Amount,
                User = user
            };

            _messageService.SendEmail(user.Email, "Info: " + transfer.Amount);

            if (user.Phone == null)
            {
                user.Money -= data.Amount;
                _transferRepository.Add(transfer);

                return PaymentStatus.Confirmed;
            }
            
            transfer.ConfirmationCode = Guid.NewGuid().ToString();
            _messageService.SendSms(user.Phone, transfer.ConfirmationCode);
            _transferRepository.Add(transfer);

            return PaymentStatus.Created;
        }
    }
}
