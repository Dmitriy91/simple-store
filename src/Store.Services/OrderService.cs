using Store.Data.Repositories;
using Store.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Store.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System;

namespace Store.Services
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

        public List<Order> GetOrdersByCustomerId(int customerId, IFiltration filtration, out int ordersFound)
        {
            IQueryable<Order> orders = null;
            string orderDate = filtration["OrderDate"];
            string sortBy = filtration.SortBy;

            if (orderDate == null)
            {
                orders = _orderRepo.GetAll()
                    .Include(o => o.OrderDetails.Select(od => od.Product))
                    .Where(o => o.CustomerId == customerId);
            }
            else
            {
                DateTime date = DateTime.Parse(orderDate).Date;

                orders = _orderRepo.GetAll()
                    .Include(o => o.OrderDetails.Select(od => od.Product))
                    .Where(o => o.CustomerId == customerId && DbFunctions.TruncateTime(o.OrderDate) == date);
            }

            ordersFound = orders.Count();

            if (sortBy == null)
                orders = orders.OrderBy(p => p.Id);
            else
                orders = orders.OrderBy(sortBy);

            return orders.Paginate(filtration.PageNumber, filtration.PageSize);
        }

        public void RemoveOrderById(int orderId)
        {
            bool orderExists = _orderRepo.Exists(o => o.Id == orderId);

            if (!orderExists)
                throw new ApplicationException("The order doesn't exist");

            _orderRepo.Delete(new Order { Id = orderId });
        }

        public void UpdateOrder(Order order)
        {
            if (order.OrderDetails == null || order.OrderDetails.Count == 0)
                throw new ApplicationException("There are no order details provided");

            bool orderExists = _orderRepo.Exists(o =>
                o.Id == order.Id &&
                o.CustomerId == order.CustomerId);

            if (!orderExists)
                throw new ApplicationException("The order doesn't exist");

            _orderDetailsRepo.Delete(od => od.OrderId == order.Id);
            //_unitOfWork.Commit();

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
        }

        public void AddOrder(Order order)
        {
            if (order.OrderDetails == null || order.OrderDetails.Count == 0)
                throw new ApplicationException("There are no order details provided");

            bool customerExists = _customerRepo.Exists(c => c.Id == order.CustomerId);

            if (!customerExists)
                throw new ApplicationException("The customer doesn't exist");

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
