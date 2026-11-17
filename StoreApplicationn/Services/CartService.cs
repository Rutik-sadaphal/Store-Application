
using System;
using System.Threading.Tasks;
using StoreApplicationn.DTOs;
using StoreApplicationn.Repositories;

namespace StoreApplicationn.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository cartRepository;
        private readonly CartCalculationService calculationService;

        public CartService(ICartRepository cartRepository)
        {
            this.cartRepository = cartRepository;
            calculationService = new CartCalculationService();
        }

        
        private CartResponseDto CalculateCart(CartResponseDto cart)
        {
            if (cart == null)
                return null;

            calculationService.Calculate(cart);

            return cart;
        }

     
        public async Task<CartResponseDto> GetCart(int userId)
        {
            var cart = await cartRepository.GetCartResponse(userId);

            return CalculateCart(cart);
        }

      
        public async Task<CartResponseDto> AddToCart(
            int userId,
            CartItemCreateDto request)
        {
            if (request == null)
                throw new InvalidOperationException(
                    "Request cannot be empty.");

            if (request.Quantity <= 0)
                throw new InvalidOperationException(
                    "Quantity must be greater than zero.");

            var product = await cartRepository.GetProductById(
                request.ProductId);

            if (product == null || !product.IsActive)
                return null;

            var cart = await cartRepository.GetCartByUserId(userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                cartRepository.AddCart(cart);
                await cartRepository.SaveChanges();
            }

            var existingItem =
                await cartRepository.GetCartItemByProductId(
                    cart.CartId,
                    request.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity += request.Quantity;
            }
            else
            {
                var cartItem = new CartItem
                {
                    CartId = cart.CartId,
                    ProductId = product.ProductId,
                    Quantity = request.Quantity,
                    AddedAt = DateTime.Now
                };

                cartRepository.AddCartItem(cartItem);
            }

            cart.UpdatedAt = DateTime.Now;

            await cartRepository.SaveChanges();

            var updatedCart =
                await cartRepository.GetCartResponse(userId);

            return CalculateCart(updatedCart);
        }

       
        public async Task<CartResponseDto> UpdateCartItem(
            int userId,
            int cartItemId,
            CartItemUpdateDto request)
        {
            if (request == null)
                throw new InvalidOperationException(
                    "Request cannot be empty.");

            if (request.Quantity <= 0)
                throw new InvalidOperationException(
                    "Quantity must be greater than zero.");

            var item =
                await cartRepository.GetCartItemByIdForUser(
                    userId,
                    cartItemId);

            if (item == null)
                return null;

            if (!item.Product.IsActive)
                throw new InvalidOperationException(
                    "This product is inactive.");

            item.Quantity = request.Quantity;
            item.Cart.UpdatedAt = DateTime.Now;

            await cartRepository.SaveChanges();

            var updatedCart =
                await cartRepository.GetCartResponse(userId);

            return CalculateCart(updatedCart);
        }

      
        public async Task<bool> RemoveCartItem(
            int userId,
            int cartItemId)
        {
            var item =
                await cartRepository.GetCartItemByIdForUser(
                    userId,
                    cartItemId);

            if (item == null)
                return false;

            item.Cart.UpdatedAt = DateTime.Now;

            cartRepository.RemoveCartItem(item);

            await cartRepository.SaveChanges();

            return true;
        }
    }
}
