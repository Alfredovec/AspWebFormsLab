using System.Web.Mvc;
using GameStore.Models.Entities;
using GameStore.Models.Enums;

namespace GameStore.Web.Infrastructure.PaymentStrategy.Interfaces
{
    public interface IPaymentContext
    {
        void SetStrategy(PaymentType type);

        ActionResult Pay(Order order);
    }
}
