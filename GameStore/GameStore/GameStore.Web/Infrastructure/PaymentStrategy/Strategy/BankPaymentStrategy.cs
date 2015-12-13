using System.Web.Mvc;
using GameStore.Models.Entities;
using GameStore.Models.Services;
using GameStore.Web.Infrastructure.PaymentStrategy.Interfaces;

namespace GameStore.Web.Infrastructure.PaymentStrategy.Strategy
{
    public class BankPaymentStrategy : IPaymentStrategy
    {
        public IStoreServices StoreServices { get; set; }

        public ActionResult Pay(Order order)
        {
            return new FileContentResult(StoreServices.OrderService.GetPdfForOrder(order.Id), "application/pdf");
        }
    }
}