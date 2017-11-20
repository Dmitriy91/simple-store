using Store.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Store.Services
{
    public interface IOrderService : IDisposable
    {
        Order GetOrderById(int orderId);

        List<Order> GetOrdersByCustomerId(int customerId, IFiltration filtration, out int ordersFound);

        void RemoveOrderById(int orderId);

        void UpdateOrder(Order order);

        void AddOrder(Order order);

        Task CommitAsync();

        void Commit();
    }
}
