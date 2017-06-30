using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Assignment.Services;
using Assignment.Web.Infrastructure.ExceptionHandling;
using BM = Assignment.Web.Models.BM;
using DTO = Assignment.Web.Models.DTO;
using AutoMapper;
using WebApi.OutputCache.V2;
using Swashbuckle.Swagger.Annotations;
using System.Net;
using Assignment.Web.Infrastructure.ValidationAttributes;

namespace Assignment.Web.Controllers.V1
{
    /// <summary>
    /// Orders
    /// </summary>
    [AutoInvalidateCacheOutput]
    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/{apiVersion}/orders")]
    public class OrdersController : ApiController
    {
        #region Fields
        private IOrderService _orderService;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="orderService"></param>
        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        #endregion

        #region Actions
        // GET: api/orders/1
        /// <summary>
        /// Get orders by customer's id
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        [Route("{customerId:int:min(1)}")]
        [CacheOutput(ServerTimeSpan = 300)]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(DTO.PaginatedData<DTO.Order>))]
        [ModelStateValidation]
        public async Task<IHttpActionResult> GetOrdersByCustomer(int customerId, [FromUri]BM.OrderFilter filter)
        {
            int ordersFound = 0;
            IFiltration filtration = Mapper.Map<BM.OrderFilter, Filtration>(filter);
            IEnumerable<Entities.Order> orders =
                await Task.Run(() => _orderService.GetOrdersByCustomerId(customerId, filtration, out ordersFound));
            IEnumerable<DTO.Order> orderDtos = Mapper.Map<IEnumerable<Entities.Order>, IEnumerable<DTO.Order>>(orders);

            var paginatedData = new DTO.PaginatedData<DTO.Order>
            {
                Collection = orderDtos,
                PagingInfo = new DTO.PagingInfo
                {
                    CurrentPage = filter.PageNumber,
                    PageSize = filter.PageSize,
                    TotalItems = ordersFound
                }
            };

            return Ok(paginatedData);
        }

        // GET: api/orders/details/1
        /// <summary>
        /// Get order by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("details/{id:int:min(1)}")]
        [CacheOutput(ServerTimeSpan = 300)]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(DTO.Order))]
        public IHttpActionResult Get(int id)
        {
            Entities.Order order = _orderService.GetOrderById(id);

            if (order == null)
                throw new BindingModelValidationException("The order does not exist.");

            DTO.Order orderDto = Mapper.Map<Entities.Order, DTO.Order>(order);

            return Ok(orderDto);
        }

        // POST: api/orders/update
        /// <summary>
        /// Update order
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update")]
        [ModelStateValidation]
        public async Task<IHttpActionResult> Update([FromBody]BM.Order order)
        {
            Entities.Order orderEntity = Mapper.Map<BM.Order, Entities.Order>(order);

            _orderService.UpdateOrder(orderEntity);
            await _orderService.CommitAsync();

            return Ok();
        }

        // POST: api/orders/add
        /// <summary>
        /// Add order
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("add")]
        [ModelStateValidation]
        public async Task<IHttpActionResult> Add([FromBody]BM.Order order)
        {
            Entities.Order orderEntity = Mapper.Map<BM.Order, Entities.Order>(order);

            _orderService.AddOrder(orderEntity);
            await _orderService.CommitAsync();

            return Ok();
        }

        // POST: api/orders/delete/1
        /// <summary>
        /// Delete order by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("delete/{id:int:min(1)}")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            _orderService.RemoveOrderById(id);
            await _orderService.CommitAsync();

            return Ok();
        }
        #endregion

        /// <summary>
        /// Dispose related resources
        /// </summary>
        /// <param name="disposing"></param>
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
