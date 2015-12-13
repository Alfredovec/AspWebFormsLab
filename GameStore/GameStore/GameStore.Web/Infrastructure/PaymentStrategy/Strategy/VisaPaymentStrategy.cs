using System.Web.Mvc;
using GameStore.Models.Entities;
using GameStore.Models.Services;
using GameStore.Web.Infrastructure.PaymentStrategy.Interfaces;
using GameStore.Web.Models;

namespace GameStore.Web.Infrastructure.PaymentStrategy.Strategy
{
    public class VisaPaymentStrategy : IPaymentStrategy
    {
        public IStoreServices StoreServices { get; set; }

        public ActionResult Pay(Order order)
        {
            var visa = new CardViewModel{CardType = GameStore.Web.Models.CardType.Visa};
            return new ViewResult {ViewName = "CardPayment", ViewData = new ViewDataDictionary(visa)};
        }
    }
}