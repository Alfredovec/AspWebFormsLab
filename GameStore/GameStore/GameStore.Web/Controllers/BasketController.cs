using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using GameStore.Models.Entities;
using GameStore.Models.Enums;
using GameStore.Models.Services;
using GameStore.Web.App_LocalResources;
using GameStore.Web.Filters;
using GameStore.Web.Infrastructure.PaymentStrategy.Interfaces;
using GameStore.Web.Models;

namespace GameStore.Web.Controllers
{
    [LocalizableAuthorize]
    public class BasketController : BaseController
    {
        private readonly IPaymentContext _context;

        private string CustomerId
        {
            get { return User.Identity.Name; }
        }

        public BasketController(IStoreServices storeServices, IPaymentContext context) : base(storeServices)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            var order = _storeServices.OrderService.GetOrderByCustomerId(CustomerId);
            return View(Mapper.Map<OrderViewModel>(order));
        }

        public ActionResult Add(string gameKey)
        {
            var game = _storeServices.GameService.GetGame(gameKey);
            _storeServices.OrderService.AddOrderDetail(CustomerId, game);
            return RedirectToAction("Index");
        }

        public ActionResult PreparePayment()
        {
            var order = _storeServices.OrderService.GetOrderByCustomerId(CustomerId);
            return View(order==null?null:order.OrderDetails.Select(Mapper.Map<OrderDetailViewModel>).ToList());
        }

        [HttpPost]
        public ActionResult PreparePayment(PaymentType type)
        {
            _context.SetStrategy(type);
            var currentOrder = _storeServices.OrderService.GetOrderByCustomerId(CustomerId);
            return _context.Pay(currentOrder);
        }

        public ActionResult Payments()
        {
            var payments = _storeServices.PaymentService.GetAllPayments().Select(Mapper.Map<PaymentViewModel>).ToList();
            return PartialView(payments);
        }

        public async Task<ActionResult> Pay(CardViewModel model)
        {
            var resultStatusTask = _storeServices.OrderService.PayOrder(Mapper.Map<CardInfo>(model), CustomerId);
            var resultMessages = new Dictionary<PaymentStatus, string>();
            resultMessages[PaymentStatus.Confirmed] = GlobalRes.PaymentConfirmed;
            resultMessages[PaymentStatus.Created] = GlobalRes.PaymentCreated;
            resultMessages[PaymentStatus.Error] = GlobalRes.PaymentError;
            resultMessages[PaymentStatus.NotEnoughMoney] = GlobalRes.NotEnoughtMoney;
            resultMessages[PaymentStatus.NotFound] = GlobalRes.NotFound;
            TempData["ResultPayment"] = resultMessages[await resultStatusTask];
            return RedirectToAction("Index");
        }

        public ActionResult Shipp(long id)
        {
            _storeServices.OrderService.ShippOrder(id);
            return RedirectToAction("Index", "Orders");
        }

        public PartialViewResult Basket()
        {
            var order = _storeServices.OrderService.GetOrderByCustomerId(CustomerId);
            if (order == null || order.OrderDetails == null || order.OrderDetails.Count == 0)
            {
                return PartialView("Basket", new BasketViewModel());
            }
            return PartialView("Basket", new BasketViewModel
            {
                Sum = _storeServices.OrderService.OrderSum(order.Id),
                Count = order.OrderDetails.Count
            });
            
        }
    }
}
