using System;
using System.Collections.Generic;
using System.Linq;
using GameStore.DALInfrastructure.Interfaces;
using GameStore.DALInfrastructure.RefModel;
using GameStore.Models.Entities;
using GameStore.Models.Repositories;

namespace GameStore.DAL.Repositories
{
    class OrderRepository : IOrderRepository
    {
        private readonly Dictionary<DatabaseName, IUnitOfWork> _unitOfWorks;
        private readonly IStoreOuterRefNavigators _refNavigators;
        private readonly UnitOfWork _unitOfWork;

        public OrderRepository(Dictionary<DatabaseName, IUnitOfWork> unitOfWorks, IStoreOuterRefNavigators refNavigators, UnitOfWork unitOfWork)
        {
            _unitOfWorks = unitOfWorks;
            _refNavigators = refNavigators;
            _unitOfWork = unitOfWork;
        }

        public void Create(Order item)
        {
            _unitOfWorks[UnitOfWork.MainDataBase].OrderRepository.Create(item);
        }

        public void Edit(Order item)
        {
            _unitOfWorks[UnitOfWork.MainDataBase].OrderRepository.Edit(item);
        }

        public void Delete(Order item)
        {
            var navigator = _refNavigators.OrderRefNavigator.DecodeGlobalId(item.Id);
            if (navigator.DatabaseName != UnitOfWork.MainDataBase)
            {
                _refNavigators.OrderRefNavigator.RemoveNavigation(item.Id, TableName.Order);
            }
            else
            {
                item.Id = navigator.OriginId;
                _unitOfWorks[UnitOfWork.MainDataBase].OrderRepository.Delete(item);
            }
        }

        public Order Get(long id)
        {
            return _unitOfWorks[UnitOfWork.MainDataBase].OrderRepository.Get(id);
        }

        public Order Get(string customerId)
        {
            foreach (var unitOfWork in _unitOfWorks)
            {
                var order = unitOfWork.Value.OrderRepository.Get(customerId);
                if (order != null)
                {
                    return order;
                }
            }
            return null;
        }

        public OrderDetail Get(string customerId, long gameId)
        {
            var navigator = _refNavigators.GameRefNavigator.DecodeGlobalId(gameId);
            if (navigator.DatabaseName != UnitOfWork.MainDataBase)
            {
                return null;
            }
            gameId = _refNavigators.GameRefNavigator.DecodeGlobalId(gameId).OriginId;
            return _unitOfWorks[UnitOfWork.MainDataBase].OrderRepository.Get(customerId, gameId);
        }

        public void PayOrder(string orderCustomerId)
        {
            _unitOfWorks[UnitOfWork.MainDataBase].OrderRepository.PayOrder(orderCustomerId);
        }

        public void ShippOrder(long id)
        {
            _unitOfWorks[UnitOfWork.MainDataBase].OrderRepository.ShippOrder(id);
        }

        public void DeleteOrderDetails(long id)
        {
            _unitOfWorks[UnitOfWork.MainDataBase].OrderRepository.DeleteOrderDetails(id);
        }

        public IEnumerable<Order> Get()
        {
            return _unitOfWorks.SelectMany(u => u.Value.OrderRepository.Get());
        }

        public IEnumerable<Order> Get(Func<Order, bool> predicate)
        {
            return Get().Where(predicate).ToList();
        }

        public void AddOrderDetails(string orderCustomerId, OrderDetail orderDetail)
        {
            var navigator = _refNavigators.GameRefNavigator.DecodeGlobalId(orderDetail.ProductId);
            if (navigator.DatabaseName != UnitOfWork.MainDataBase)
            {
                var game = _unitOfWork.GameRepository.Get(orderDetail.ProductId);
                _unitOfWork.GameRepository.Edit(game);
                orderDetail.ProductId = game.Id;
            }
            orderDetail.ProductId = _refNavigators.GameRefNavigator.DecodeGlobalId(orderDetail.ProductId).OriginId;
            _unitOfWorks[UnitOfWork.MainDataBase].OrderRepository.AddOrderDetails(orderCustomerId, orderDetail);
        }

        public void EditOrderDetail(OrderDetail orderDetail)
        {
            _unitOfWorks[UnitOfWork.MainDataBase].OrderRepository.EditOrderDetail(orderDetail);
        }
    }
}
