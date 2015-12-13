using System.Collections.Generic;
using GameStore.Models.Entities;
using GameStore.Models.Enums;

namespace GameStore.Models.Services
{
    public interface IPaymentService
    {
        IEnumerable<Payment> GetAllPayments();

        Payment GetPayment(long id);

        Payment GetPayment(PaymentType type);

        void CreatePayment(Payment payment);
    }
}
