using System.Threading.Tasks;
using StoreApplicationn.DTOs;
using StoreApplicationn.Services;
using System.Web.Http;

namespace StoreApplicationn.Controllers
{
    [RoutePrefix("api/checkouts")]
    public class CheckoutsController : ApiController
    {
        private readonly ICheckoutServices checkoutService;

        public CheckoutsController()
        {
            checkoutService = new CheckoutService();
        }

        [HttpPost]
        [Route("validate")]
        public async Task<IHttpActionResult> ValidateCheckout(
            [FromBody] CheckoutRequestDto request)
        {
            if (request == null)
                return BadRequest("Checkout request is required.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            CheckoutResponseDto result = await checkoutService.ValidateCheckout(
                request.UserId,
                request.BillingAddressId,
                request.ShippingAddressId);

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result);
        }
    }
}