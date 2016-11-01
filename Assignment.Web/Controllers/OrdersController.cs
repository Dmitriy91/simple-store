using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Assignment.Entities;
using Assignment.Services;
using Assignment.Web.Infrastructure;
using Assignment.Web.Infrastructure.ExceptionHandling;
using Assignment.Web.Models;
using AutoMapper;
using WebApi.OutputCache.V2;

namespace Assignment.Web.Controllers
{
    [AutoInvalidateCacheOutput]
    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/orders")]
    public class OrdersController : ApiController
    {
        #region Fields
        private IOrderService _orderService;
        #endregion

        #region Constructors
        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        #endregion

        #region Actions
        // GET: api/orders/1
        [Route("{customerId:int:min(1)}")]
        [CacheOutput(ServerTimeSpan = 300)]
        public async Task<IHttpActionResult> GetOrdersByCustomer(int customerId, [FromUri]OrderFilterBM filter)
        {
            filter = filter ?? new OrderFilterBM();

            if (!ModelState.IsValid)
                throw new BindingModelValidationException(this.GetModelStateErrorMessage());

            int ordersFound = 0;
            IFiltration filtration = Mapper.Map<OrderFilterBM, Filtration>(filter);
            IEnumerable<Order> orders =
                await Task.Run(() => _orderService.GetOrdersByCustomerId(customerId, filtration, out ordersFound));
            IEnumerable<OrderDTO> orderDtos = Mapper.Map<IEnumerable<Order>, IEnumerable<OrderDTO>>(orders);

            return Ok(new
            {
                Orders = orderDtos,
                PagingInfo = new PagingInfoDTO
                {
                    CurrentPage = filter.PageNumber,
                    PageSize = filter.PageSize,
                    TotalItems = ordersFound
                }
            });
        }

        // GET: api/orders/details/1
        [Route("details/{id:int:min(1)}")]
        [CacheOutput(ServerTimeSpan = 300)]
        public IHttpActionResult Get(int id)
        {
            Order order = _orderService.GetOrderById(id);

            if (order == null)
                throw new BindingModelValidationException("The order does not exist.");

            OrderDTO orderDto = Mapper.Map<Order, OrderDTO>(order);

            return Ok(orderDto);
        }

        // POST: api/orders/update
        [HttpPost]
        [Route("update")]
        public async Task<IHttpActionResult> Update([FromBody]OrderBM orderBindingModel)
        {
            if (!ModelState.IsValid)
                throw new BindingModelValidationException(this.GetModelStateErrorMessage());

            Order order = Mapper.Map<OrderBM, Order>(orderBindingModel);

            if (_orderService.UpdateOrder(order))
                await _orderService.CommitAsync();

            return Ok();
        }

        // POST: api/orders/add
        [HttpPost]
        [Route("add")]
        public async Task<IHttpActionResult> Add([FromBody]OrderBM orderBindingModel)
        {
            if (!ModelState.IsValid)
                throw new BindingModelValidationException(this.GetModelStateErrorMessage());

            Order order = Mapper.Map<OrderBM, Order>(orderBindingModel);

            if (_orderService.AddOrder(order))
                await _orderService.CommitAsync();

            return Ok();
        }

        // POST: api/orders/delete/1
        [HttpPost]
        [Route("delete/{id:int:min(1)}")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            if (_orderService.RemoveOrderById(id))
                await _orderService.CommitAsync();

            return Ok();
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_orderService != null)
                {
                    _orderService.Dispose();
                    _orderService = null;
                }
            }

            base.Dispose(disposing);
        }
    }
}
