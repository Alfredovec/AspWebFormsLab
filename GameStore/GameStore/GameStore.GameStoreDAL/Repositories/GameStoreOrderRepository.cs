using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using GameStore.Models.Entities;
using GameStore.Models.Enums;
using GameStore.Models.Repositories;

namespace GameStore.GameStoreDAL.Repositories
{
    class GameStoreOrderRepository : IOrderRepository
    {
        private readonly GameStoreContext _db;

        public GameStoreOrderRepository(GameStoreContext db)
        {
            _db = db;
        }

        public void Create(Order item)
        {
            _db.Orders.Add(item);
        }

        public void Edit(Order item)
        {
            _db.Entry(item).State = EntityState.Modified;
        }

        public void Delete(Order item)
        {
            _db.Orders.Remove(item);
        }

        public Order Get(long id)
        {
            return _db.Orders.Include(o=>o.OrderDetails).Include(o => o.OrderDetails.Select(od => od.Game)).First(o => o.Id == id);
        }

        public Order Get(string customerId)
        {
            return _db.Orders.Include(o => o.OrderDetails).Include(o => o.OrderDetails.Select(od => od.Game))
                .FirstOrDefault(o => o.CustomerId == customerId && o.OrderStatus == OrderStatus.Created);
        }

        public OrderDetail Get(string customerId, long gameId)
        {
            return _db.OrderDetails.FirstOrDefault(o => o.Order.CustomerId == customerId && o.ProductId == gameId);
        }

        public IEnumerable<Order> Get()
        {
            var orders = _db.Orders.Include(o => o.OrderDetails).Include(o => o.OrderDetails.Select(od => od.Game)).ToList();
            return orders;
        }

        public IEnumerable<Order> Get(Func<Order, bool> predicate)
        {
            return _db.Orders.Include(o => o.OrderDetails).Include(o => o.OrderDetails.Select(od => od.Game)).Where(predicate).ToList();
        }

        public void PayOrder(string orderCustomerId)
        {
            var order = Get(orderCustomerId);
            order.OrderStatus = OrderStatus.Payed;
        }

        public void ShippOrder(long id)
        {
            var order = Get(id);
            order.ShippedDate = DateTime.UtcNow;
            order.OrderStatus = OrderStatus.Shipped;
        }

        public void DeleteOrderDetails(long id)
        {
            var order = _db.OrderDetails.Find(id);
            _db.OrderDetails.Remove(order);
        }

        public void AddOrderDetails(string orderCustomerId, OrderDetail orderDetail)
        {
            var order =
                _db.Orders.FirstOrDefault(o => o.CustomerId == orderCustomerId && o.OrderStatus == OrderStatus.Created) ??
                new Order {CustomerId = orderCustomerId, Date = DateTime.UtcNow, OrderStatus = OrderStatus.Created};
            orderDetail.Order = order;
            _db.OrderDetails.Add(orderDetail);
        }

        public void EditOrderDetail(OrderDetail orderDetail)
        {
            _db.Entry(orderDetail).State = EntityState.Modified;
        }
    }
}
