
using System;
using System.Net;
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

      
        [HttpGet]
        [Route("{userId:int}")]
        public async Task<IHttpActionResult> GetCart(int userId)
        {
            if (userId <= 0)
                return BadRequest("User ID must be greater than zero.");

            var cart = await cartService.GetCart(userId);

            if (cart == null)
                return NotFound(); 

            return Ok(cart);
        }

        
        [HttpPost]
        [Route("{userId:int}/items")]
        public async Task<IHttpActionResult> AddToCart(
            int userId,
            [FromBody] CartItemCreateDto request)
        {
            if (userId <= 0)
                return BadRequest("User ID must be greater than zero.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState); 

            try
            {
                var cart = await cartService.AddToCart(userId, request);

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

        
        [HttpPut]
        [Route("{userId:int}/items/{cartItemId:int}")]
        public async Task<IHttpActionResult> UpdateCartItem(
            int userId,
            int cartItemId,
            [FromBody] CartItemUpdateDto request)
        {
            if (userId <= 0 || cartItemId <= 0)
                return BadRequest("User ID and Cart Item ID must be greater than zero.");

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

        
        [HttpDelete]
        [Route("{userId:int}/items/{cartItemId:int}")]
        public async Task<IHttpActionResult> RemoveCartItem(
            int userId,
            int cartItemId)
        {
            if (userId <= 0 || cartItemId <= 0)
                return BadRequest("User ID and Cart Item ID must be greater than zero.");

            bool removed = await cartService.RemoveCartItem(
                userId, cartItemId);

            if (!removed)
                return NotFound();

            return StatusCode(HttpStatusCode.NoContent); 
        }
    }
}
