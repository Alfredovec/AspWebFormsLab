using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AutoMapper;
using GameStore.DALInfrastructure.Interfaces;
using GameStore.DALInfrastructure.RefModel;
using GameStore.Models.Entities;
using GameStore.Models.Repositories;

namespace GameStore.NorthwindDAL.Repositories
{
    class NorthwinOrderRepository : IOrderRepository
    {
        private readonly Entities _db;

        private readonly IStoreOuterRefNavigators _refNavigators;

        public NorthwinOrderRepository(Entities db, IStoreOuterRefNavigators refNavigators)
        {
            _db = db;
            _refNavigators = refNavigators;
        }

        public void Create(Order item)
        {
            throw new NotImplementedException();
        }

        public void Edit(Order item)
        {
            throw new NotImplementedException();
        }

        public void Delete(Order item)
        {
            throw new NotImplementedException();
        }

        public Order Get(long id)
        {
            var order = _db.Orders.Include(o => o.Order_Details).First(o => o.OrderID == id);
            return Mapper.Map<Order>(order);
        }

        public Order Get(string customerId)
        {
            var order = _db.Orders.Include(o => o.Order_Details).FirstOrDefault(o => o.CustomerID == customerId);
            return Mapper.Map<Order>(order);
        }

        public OrderDetail Get(string customerId, long gameId)
        {
            throw new NotImplementedException();
        }

        public void PayOrder(string orderCustomerId)
        {
            throw new NotImplementedException();
        }

        public void ShippOrder(long id)
        {
            throw new NotImplementedException();
        }

        public void DeleteOrderDetails(long id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Order> Get()
        {
            var orders = _db.Orders.Include(o => o.Order_Details).Select(Mapper.Map<Order>).ToList();
            foreach (var order in orders)
            {
                foreach (var orderDetail in order.OrderDetails)
                {
                    orderDetail.Game = _refNavigators.GameRefNavigator.SetGlobalRefWithCheckDeletion(orderDetail.Game, DatabaseName.Northwind);
                }
            }
            return orders;
        }

        public IEnumerable<Order> Get(Func<Order, bool> predicate)
        {
            return Get().Where(predicate).ToList();
        }

        public void AddOrderDetails(string orderCustomerId, OrderDetail orderDetail)
        {
            throw new NotImplementedException();
        }

        public void EditOrderDetail(OrderDetail orderDetail)
        {
            throw new NotImplementedException();
        }
    }
}