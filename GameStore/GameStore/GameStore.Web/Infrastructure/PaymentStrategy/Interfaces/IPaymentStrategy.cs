using System.Web.Mvc;
using GameStore.Models.Entities;
using GameStore.Models.Services;

namespace GameStore.Web.Infrastructure.PaymentStrategy.Interfaces
{
    public interface IPaymentStrategy
    {
        IStoreServices StoreServices { get; set; }

        ActionResult Pay(Order order);
    }
}