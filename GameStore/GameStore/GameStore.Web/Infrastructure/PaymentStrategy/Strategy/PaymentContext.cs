using System.Collections.Generic;
using System.Web.Mvc;
using GameStore.Models.Entities;
using GameStore.Models.Enums;
using GameStore.Models.Services;
using GameStore.Web.Infrastructure.PaymentStrategy.Interfaces;

namespace GameStore.Web.Infrastructure.PaymentStrategy.Strategy
{
    public class PaymentContext : IPaymentContext
    {
        private readonly IStoreServices _storeService;
        private PaymentType _currenPaymentType;
        private readonly Dictionary<PaymentType, IPaymentStrategy> _strategies;

        public PaymentContext(IStoreServices storeServices)
        {
            _storeService = storeServices;
            _strategies = new Dictionary<PaymentType, IPaymentStrategy>();
            _strategies[PaymentType.Bank] = new BankPaymentStrategy();
            _strategies[PaymentType.Ibox] = new IboxPaymentStrategy();
            _strategies[PaymentType.Visa] = new VisaPaymentStrategy();
            _strategies[PaymentType.MasterCard] = new MasterCardPaymentStrategy();
        }

        public void SetStrategy(PaymentType type)
        {
            _currenPaymentType = type;
        }

        public ActionResult Pay(Order order)
        {
            var strategy = _strategies[_currenPaymentType];
            strategy.StoreServices = _storeService;
            return strategy.Pay(order);
        }
    }
}