using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameStore.Models.Entities;

namespace GameStore.Models.Services
{
    public interface IOrderService
    {
        Order GetOrderByCustomerId(string id);

        void AddOrderDetail(string customerId, OrderDetail detail);

        void AddOrderDetail(string customerId, Game game, short quantity = 1);

        decimal OrderSum(long orderId);

        byte[] GetPdfForOrder(long orderId);

        IEnumerable<Order> GetHistory(DateTime startDate, DateTime endDate);

        Order GetOrder(long id);

        void ShippOrder(long id);

        Task<PaymentStatus> PayOrder(CardInfo card, string customerId);
    }
}
