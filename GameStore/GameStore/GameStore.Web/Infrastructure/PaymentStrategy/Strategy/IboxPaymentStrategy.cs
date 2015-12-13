using System.Web.Mvc;
using GameStore.Models.Entities;
using GameStore.Models.Services;
using GameStore.Web.Infrastructure.PaymentStrategy.Interfaces;
using GameStore.Web.Models;

namespace GameStore.Web.Infrastructure.PaymentStrategy.Strategy
{
    public class IboxPaymentStrategy : IPaymentStrategy
    {
        public IStoreServices StoreServices { get; set; }

        public ActionResult Pay(Order order)
        {
            var ibox = new IboxViewModel
            {
                CustomerId = order.CustomerId,
                OrderId = order.Id,
                Sum = StoreServices.OrderService.OrderSum(order.Id)
            };

            return new ViewResult { ViewName = "Ibox", ViewData = new ViewDataDictionary(ibox) };
        }
    }
}