
using System.Threading.Tasks;
using System.Web.Http;
using StoreApplicationn.DTOs;
using StoreApplicationn.Services;

namespace StoreApplicationn.Controllers
{
    [RoutePrefix("api/orders")]
    public class OrdersApiController : ApiController
    {
        private readonly IOrderService orderService;

        public OrdersApiController()
        {
            orderService = new OrderService();
        }

        [HttpPost]
        [Route("create")]
        public async Task<IHttpActionResult> CreateOrder(
            [FromBody] OrderCreateDto request)
        {
            if (request == null)
                return BadRequest("Order details are required.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            OrderResponseDto response =
                await orderService.CreateOrder(request);

            if (response == null)
                return InternalServerError();

            if (response.OrderId <= 0)
                return BadRequest(response.Message);

            return Ok(response);
        }
        [HttpGet]
        [Route("{orderId:int}")]
        public async Task<IHttpActionResult> GetOrderDetails(int orderId)
        {
            if (orderId <= 0)
                return BadRequest("Please enter a valid Order ID.");

            var response = await orderService.GetOrderDetails(orderId);

            if (response == null)
                return NotFound();

            return Ok(response);
        }
    }
}
