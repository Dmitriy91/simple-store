using Assignment.Entities;
using Assignment.Services;
using Assignment.Web.Models;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace Assignment.Web.Controllers
{
    [Authorize]
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
        // GET: api/orders/customer/1
        [Route("orders/{id:int:min(1)}")]
        public async Task<IHttpActionResult> GetOrdersByCustomer(int id)
        {
            IEnumerable<Order> orders = await Task<IEnumerable<Order>>.Run(() =>
            {
                return _orderService.GetOrdersByCustomerId(id);
            });

            IEnumerable<OrderDto> ordersDtos = Mapper.Map<IEnumerable<Order>, IEnumerable<OrderDto>>(orders);

            return Ok(ordersDtos);
        }

        // GET: api/orders/details/1
        [Route("details/{id:int:min(1)}")]
        public IHttpActionResult Get(int id)
        {
            Order order = _orderService.GetOrderById(id);

            if (order == null)
                return BadRequest();

            OrderDto orderDto = Mapper.Map<Order, OrderDto>(order);

            return Ok(orderDto);
        }

        // POST: api/orders/update/1
        [HttpPost]
        [Route("update")]
        public async Task<IHttpActionResult> Update([FromBody]OrderBindingModel orderBindingModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Order order = Mapper.Map<OrderBindingModel, Order>(orderBindingModel);

            if (_orderService.UpdateOrder(order))
            {
                await _orderService.CommitAsync();

                return Ok();
            }

            return BadRequest();
        }

        // POST: api/orders/add
        [HttpPost]
        [Route("add")]
        public async Task<IHttpActionResult> Add([FromBody]OrderBindingModel orderBindingModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Order order = Mapper.Map<OrderBindingModel, Order>(orderBindingModel);

            _orderService.AddOrder(order);
            await _orderService.CommitAsync();

            return Ok();
        }

        // POST: api/orders/delete/1
        [HttpPost]
        [Route("delete/{id:int:min(1)}")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            if (_orderService.RemoveOrderById(id))
            {
                await _orderService.CommitAsync();

                return Ok();
            }

            return BadRequest();
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
