
using System.Threading.Tasks;
using System.Web.Http;
using StoreApplicationn.DTOs;
using StoreApplicationn.Services;

namespace StoreApplicationn.Controllers
{
    [RoutePrefix("api/payments")]
    public class PaymentsApiController : ApiController
    {
        private readonly IPaymentService paymentService;

        public PaymentsApiController()
        {
            paymentService = new PaymentService();
        }

        [HttpPost]
        [Route("process")]
        public async Task<IHttpActionResult> ProcessPayment(
            [FromBody] PaymentRequestDto request)
        {
            if (request == null)
                return BadRequest("Payment request is required.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await paymentService.ProcessPayment(request);
                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
