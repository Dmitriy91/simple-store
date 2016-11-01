using Assignment.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Assignment.Services
{
    public interface IOrderService : IDisposable
    {
        Order GetOrderById(int orderId);

        IEnumerable<Order> GetOrdersByCustomerId(int customerId, IFiltration filtration, out int ordersFound);

        bool RemoveOrderById(int orderId);

        bool UpdateOrder(Order order);

        bool AddOrder(Order order);

        Task CommitAsync();

        void Commit();
    }
}
