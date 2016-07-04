using Assignment.Data.Repositories;
using Assignment.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Assignment.Data;
using System.Data.Entity;
using System.Linq;
using System;

namespace Assignment.Services
{
    public class OrderService : IOrderService
    {
        #region Fields
        private IRepository<Order> _orderRepo;
        private IRepository<OrderDetails> _orderDetailsRepo;
        private IRepository<Product> _productRepo;
        private IRepository<Customer> _customerRepo;
        private IUnitOfWork _unitOfWork;
        #endregion

        #region Constructors
        public OrderService(IRepository<Order> orderRepo,
            IRepository<OrderDetails> orderDetailsRepo,
            IRepository<Product> productRepo,
            IRepository<Customer> customerRepo,
            IUnitOfWork unitOfWork)
        {
            _orderDetailsRepo = orderDetailsRepo;
            _orderRepo = orderRepo;
            _productRepo = productRepo;
            _customerRepo = customerRepo;
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
            IEnumerable<Order> orders = _orderRepo.GetAll()
                .Include(o => o.OrderDetails.Select(od => od.Product)) // Note: Should be optimized
                .Where(o => o.CustomerId == customerId)
                .ToList();

            return orders;
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
            if (order.OrderDetails == null || order.OrderDetails.Count == 0)
                return false;

            bool orderExists = _orderRepo.Exists(o =>
                o.Id == order.Id &&
                o.CustomerId == order.CustomerId);

            if (!orderExists)
                return false;

            _orderDetailsRepo.Delete(od => od.OrderId == order.Id);
            _unitOfWork.Commit();

            foreach (OrderDetails orderItem in order.OrderDetails)
            {
                Product product = _productRepo.GetById(orderItem.ProductId);

                if (product != null)
                {
                    orderItem.OrderId = order.Id;
                    orderItem.Product = product;
                    orderItem.UnitPrice = product.UnitPrice;
                    _orderDetailsRepo.Add(orderItem);
                }
            }

            return true;
        }

        public bool AddOrder(Order order)
        {
            if (order.OrderDetails == null || order.OrderDetails.Count == 0)
                return false;

            bool customerExists = _customerRepo.Exists(c => c.Id == order.CustomerId);

            if (!customerExists)
                return false;

            order.OrderDate = DateTime.Now;

            foreach (OrderDetails orderItem in order.OrderDetails)
            {
                Product product = _productRepo.GetById(orderItem.ProductId);

                if (product != null)
                {
                    orderItem.UnitPrice = product.UnitPrice;
                    orderItem.Order = order;
                    orderItem.Product = product;
                }
            }

            _orderRepo.Add(order);

            return true;
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
