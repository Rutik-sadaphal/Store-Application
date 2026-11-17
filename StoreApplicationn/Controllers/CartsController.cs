using System;
using System.Threading.Tasks;
using System.Web.Http;
using StoreApplicationn.DTOs;
using StoreApplicationn.Services;
using StoreApplicationn.Repositories;

namespace StoreApplicationn.Controllers
{
    [RoutePrefix("api/carts")]
    public class CartsController : ApiController
    {
        private readonly ICartService cartService;

        public CartsController()
        {
            ICartRepository cartRepository = new CartRepository();
            cartService = new CartService(cartRepository);
        }

        // GET: api/carts/1
        [HttpGet]
        [Route("{userId:int}")]
        public async Task<IHttpActionResult> GetCart(int userId)
        {
            var cart = await cartService.GetCart(userId);

            if (cart == null)
                return NotFound();

            return Ok(cart);
        }

        // POST: api/carts/1/items
        [HttpPost]
        [Route("{userId:int}/items")]
        public async Task<IHttpActionResult> AddToCart(
            int userId,
            [FromBody] CartItemCreateDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var cart = await cartService.AddToCart(
                    userId, request);

                if (cart == null)
                    return BadRequest(
                        "Product does not exist or is inactive.");

                return Ok(cart);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/carts/1/items/1
        [HttpPut]
        [Route("{userId:int}/items/{cartItemId:int}")]
        public async Task<IHttpActionResult> UpdateCartItem(
            int userId,
            int cartItemId,
            [FromBody] CartItemUpdateDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var cart = await cartService.UpdateCartItem(
                    userId, cartItemId, request);

                if (cart == null)
                    return NotFound();

                return Ok(cart);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/carts/1/items/1
        [HttpDelete]
        [Route("{userId:int}/items/{cartItemId:int}")]
        public async Task<IHttpActionResult> RemoveCartItem(
            int userId,
            int cartItemId)
        {
            bool removed = await cartService.RemoveCartItem(
                userId, cartItemId);

            if (!removed)
                return NotFound();

            return Ok("Cart item removed successfully.");
        }
    }
}