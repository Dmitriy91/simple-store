using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Store.Services;
using Store.Web.Infrastructure.ExceptionHandling;
using BM = Store.Web.Models.BM;
using DTO = Store.Contracts;
using AutoMapper;
using WebApi.OutputCache.V2;
using Swashbuckle.Swagger.Annotations;
using System.Net;
using Store.Web.Infrastructure.ValidationAttributes;

namespace Store.Web.Controllers.V1
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
        private IMapper _mapper;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="orderService"></param>
        /// <param name="mapper"></param>
        public OrdersController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
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
            IFiltration filtration = _mapper.Map<BM.OrderFilter, Filtration>(filter);
            List<Entities.Order> orders =
                await Task.Run(() => _orderService.GetOrdersByCustomerId(customerId, filtration, out ordersFound));
            List<DTO.Order> orderDtos = _mapper.Map<List<Entities.Order>, List<DTO.Order>>(orders);

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

            DTO.Order orderDto = _mapper.Map<Entities.Order, DTO.Order>(order);

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
            Entities.Order orderEntity = _mapper.Map<BM.Order, Entities.Order>(order);

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
            Entities.Order orderEntity = _mapper.Map<BM.Order, Entities.Order>(order);

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
