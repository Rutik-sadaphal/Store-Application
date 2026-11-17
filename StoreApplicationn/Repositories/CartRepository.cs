using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using StoreApplicationn.DTOs;

namespace StoreApplicationn.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ShoppingAppDBEntities db;

        public CartRepository()
        {
            db = new ShoppingAppDBEntities();
        }

        
        public async Task<Product> GetProductById(int productId)
        {
            return await db.Products
                .FirstOrDefaultAsync(p => p.ProductId == productId);
        }

       
        public async Task<Cart> GetCartByUserId(int userId)
        {
            return await db.Carts
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

     
        public async Task<CartItem> GetCartItemByProductId(
            int cartId,
            int productId)
        {
            return await db.CartItems
                .FirstOrDefaultAsync(i =>
                    i.CartId == cartId &&
                    i.ProductId == productId);
        }

       
        public async Task<CartItem> GetCartItemByIdForUser(
            int userId,
            int cartItemId)
        {
            return await db.CartItems
                .Include(i => i.Cart)
                .Include(i => i.Product)
                .FirstOrDefaultAsync(i =>
                    i.CartItemId == cartItemId &&
                    i.Cart.UserId == userId);
        }

      
        public void AddCart(Cart cart)
        {
            db.Carts.Add(cart);
        }

        public void AddCartItem(CartItem cartItem)
        {
            db.CartItems.Add(cartItem);
        }

        
        public void RemoveCartItem(CartItem cartItem)
        {
            db.CartItems.Remove(cartItem);
        }

      
        public async Task SaveChanges()
        {
            await db.SaveChangesAsync();
        }

      
        public async Task<CartResponseDto> GetCartResponse(int userId)
        {
            var cart = await db.Carts
                .Include(c => c.CartItems.Select(i => i.Product))
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
                return null;

            return MapCart(cart);
        }

       
        private CartResponseDto MapCart(Cart cart)
        {
            var response = new CartResponseDto
            {
                CartId = cart.CartId,
                UserId = cart.UserId,

                Items = cart.CartItems.Select(item =>
                    new CartItemResponseDto
                    {
                        CartItemId = item.CartItemId,
                        ProductId = item.ProductId,
                        ProductName = item.Product.ProductName,
                        UnitPrice = item.Product.Price,
                        Quantity = item.Quantity,
                        LineTotal = item.Product.Price * item.Quantity
                    }).ToList()
            };

            response.Subtotal =
                response.Items.Sum(item => item.LineTotal);

            return response;
        }
    }
}