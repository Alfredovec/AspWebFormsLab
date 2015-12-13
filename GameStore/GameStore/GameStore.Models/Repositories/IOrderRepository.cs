using GameStore.Models.Entities;

namespace GameStore.Models.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        void AddOrderDetails(string orderCustomerId, OrderDetail orderDetail);

        void EditOrderDetail(OrderDetail orderDetail);

        Order Get(string customerId);

        OrderDetail Get(string customerId, long gameId);

        void PayOrder(string orderCustomerId);

        void ShippOrder(long id);

        void DeleteOrderDetails(long id);
    }
}