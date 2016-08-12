using Assignment.Entities;
using Assignment.Services;
using Assignment.Web.Models;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace Assignment.Web.Controllers
{
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
        public async Task<IHttpActionResult> GetOrdersByCustomer(int customerId)
        {
            IEnumerable<Order> orders = await Task.Run(() => _orderService.GetOrdersByCustomerId(customerId));
            IEnumerable<OrderDto> orderDtos = Mapper.Map<IEnumerable<Order>, IEnumerable<OrderDto>>(orders);

            return Ok(orderDtos);
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

        // POST: api/orders/update
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

            if (_orderService.AddOrder(order))
            {
                await _orderService.CommitAsync();
                return Ok();
            }

            return BadRequest();
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

            return BadRequest("Ordered product can not be deleted!");
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
