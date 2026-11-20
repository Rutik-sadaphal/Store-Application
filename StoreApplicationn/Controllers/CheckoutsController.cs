using System.Threading.Tasks;
using StoreApplicationn.DTOs;
using StoreApplicationn.Services;
using System.Web.Http;

namespace StoreApplicationn.Controllers
{
    [RoutePrefix("api/checkouts")]
        public class CheckoutsController : ApiController
        {
            private readonly CheckoutService checkoutService;

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
                {
                    return BadRequest("Checkout Request is Required.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                string result = await checkoutService.ValidateCheckout(request.UserId,
                request.BillingAddressId,
                request.ShippingAddressId);

                if (result != null)
                {
                    return BadRequest(result);
                }

                return Ok(new
                {
                    Message = "Checkout validation successful."
                });

            }

        }
    
}