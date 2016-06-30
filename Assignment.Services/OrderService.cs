using Assignment.Data.Repositories;
using Assignment.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Assignment.Data;
using System.Data.Entity;
using System.Linq;

namespace Assignment.Services
{
    public class OrderService : IOrderService
    {
        #region Fields
        private IRepository<Order> _orderRepo;
        private IRepository<OrderDetails> _orderDetailsRepo;
        private IUnitOfWork _unitOfWork;
        #endregion

        #region Constructors
        public OrderService(IRepository<Order> orderRepo, IRepository<OrderDetails> orderDetailsRepo, IUnitOfWork unitOfWork)
        {
            _orderDetailsRepo = orderDetailsRepo;
            _orderRepo = orderRepo;
            _unitOfWork = unitOfWork;
        }
        #endregion

        #region Methods
        public Order GetOrderById(int orderId)
        {
            return _orderRepo.GetAll()
                .Include(o => o.OrderDetails)
                .FirstOrDefault(o => o.Id == orderId);
        }

        public IEnumerable<Order> GetOrdersByCustomerId(int customerId)
        {
            return _orderRepo.GetAll()
                .Include(o => o.OrderDetails)
                .Where(o => o.CustomerId == customerId);
        }

        public bool RemoveOrderById(int orderId)
        {
            bool orderExists = _orderRepo.Exists(o => o.Id == orderId);

            if (orderExists)
            {
                _orderRepo.Delete(new Order { Id = orderId });
                return true;
            }

            return false;
        }

        public bool UpdateOrder(Order order)
        {
            bool orderExists = _orderRepo.Exists(o => o.Id == order.Id);

            if (!orderExists)
                return false;

            if (order.OrderDetails != null)
                _orderDetailsRepo.Update(order.OrderDetails.ToArray());

            _orderRepo.Update(order);

            return true;
        }

        public void AddOrder(Order order)
        {
            _orderRepo.Add(order);
        }

        async public Task CommitAsync()
        {
            await _unitOfWork.CommitAsync();
        }

        public void Commit()
        {
            _unitOfWork.Commit();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_unitOfWork != null)
                {
                    _unitOfWork.Dispose();
                    _unitOfWork = null;
                }
            }
        }
        #endregion
    }
}
