using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GameStore.BLL.Interfaces;
using GameStore.BLL.Observer;
using GameStore.BLL.PaymentService;
using GameStore.Models.Entities;
using GameStore.Models.Repositories;
using GameStore.Models.Services;
using GameStore.Models.Utils;
using iTextSharp.text;
using iTextSharp.text.pdf;
using IPaymentService = GameStore.BLL.PaymentService.IPaymentService;
using PaymentStatus = GameStore.Models.Entities.PaymentStatus;

namespace GameStore.BLL.Services
{
    class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public OrderService(IUnitOfWork unitOfWork, ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public void AddOrderDetail(string orderCustomerId, OrderDetail detail)
        {
            using (_logger.LogPerfomance())
            {
                _unitOfWork.OrderRepository.AddOrderDetails(orderCustomerId, detail);
            }
        }

        public void AddOrderDetail(string customerId, Game game, short quantity = 1)
        {
            using (_logger.LogPerfomance())
            {
                var orderDetail = _unitOfWork.OrderRepository.Get(customerId, game.Id);
                if (orderDetail == null)
                {
                    orderDetail = new OrderDetail { ProductId = game.Id, Quantity = quantity };
                    _unitOfWork.OrderRepository.AddOrderDetails(customerId, orderDetail);
                }
                else
                {
                    if (orderDetail.Quantity == -quantity)
                    {
                        _unitOfWork.OrderRepository.DeleteOrderDetails(orderDetail.Id);
                    }
                    else
                    {
                        orderDetail.Quantity += quantity;
                        _unitOfWork.OrderRepository.EditOrderDetail(orderDetail);
                    }
                }
                _unitOfWork.Save();
            }
        }

        public Order GetOrderByCustomerId(string id)
        {
            using (_logger.LogPerfomance())
            {
                var order = _unitOfWork.OrderRepository.Get(id);
                return order;
            }
        }


        public decimal OrderSum(long orderId)
        {
            using (_logger.LogPerfomance())
            {
                var order = _unitOfWork.OrderRepository.Get(orderId);
                return OrderSum(order);
            }
        }

        public byte[] GetPdfForOrder(long orderId)
        {
            var order = _unitOfWork.OrderRepository.Get(orderId);
            MemoryStream stream = new MemoryStream();
            Document document = new Document();
            PdfWriter.GetInstance(document, stream).CloseStream = false;

            document.Open();
            string paragraph = string.Format("OrderID: {1}{0}Order sum:{2}{0}Customer: {3}",
                Environment.NewLine,
                order.Id,
                OrderSum(order),
                order.CustomerId);
            document.Add(new Paragraph(paragraph));
            document.Close();

            return stream.ToArray();
        }

        private decimal OrderSum(Order order)
        {
            return order.OrderDetails.Aggregate(0m, (s, o) => s += o.Quantity * o.Game.Price * (1 - o.Discount));
        }

        public IEnumerable<Order> GetHistory(DateTime startDate, DateTime endDate)
        {
            using (_logger.LogPerfomance())
            {
                var orders = _unitOfWork.OrderRepository.Get(o => o.Date >= startDate && o.Date <= endDate);
                return orders;
            }
        }

        public Order GetOrder(long id)
        {
            using (_logger.LogPerfomance())
            {
                return _unitOfWork.OrderRepository.Get(id);
            }
        }

        public void ShippOrder(long id)
        {
            using (_logger.LogPerfomance())
            {
                _unitOfWork.OrderRepository.ShippOrder(id);
                _unitOfWork.Save();
            }
        }

        public async Task<PaymentStatus> PayOrder(CardInfo card, string customerId)
        {
            using (_logger.LogPerfomance())
            {
                var order = GetOrderByCustomerId(customerId);
                var service = new PaymentServiceClient();
                var expirationMonth = Convert.ToInt32(card.DateOfExpire.Split('/')[0]);
                var expirationYear = Convert.ToInt32(card.DateOfExpire.Split('/')[1]);
                var data = new PaymentData
                {
                    Cvv2 = card.Cvc.ToString(),
                    Name = card.Name,
                    AccountNumber = card.CardNumber,
                    Amount = OrderSum(order),
                    Owner = card.Name,
                    ExpirationMonth = expirationMonth,
                    ExpirationYear = expirationYear
                };
                
                PaymentStatus status;
                switch (card.CardType)
                {
                    case CardType.Visa:
                        status = (PaymentStatus) await service.PayByVisaAsync(data);
                        break;
                    case CardType.MasterCard:
                        status = (PaymentStatus) await service.PayByMasterCardAsync(data);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                if (status == PaymentStatus.Confirmed)
                {
                    _unitOfWork.OrderRepository.PayOrder(customerId);
                    _unitOfWork.Save();
                    IObservable observable = Observable.CreateObservable(_unitOfWork);
                    observable.NotifyObserver(order);
                }
                _logger.LogInfo(string.Format("Transfer status: {0} for user: {1}", status, customerId));
                return status;
            }
        }
    }
}
