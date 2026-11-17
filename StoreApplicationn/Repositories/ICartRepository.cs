using System.Threading.Tasks;
using StoreApplicationn.DTOs;

namespace StoreApplicationn.Repositories
{
    public interface ICartRepository
    {
        Task<Product> GetProductById(int productId);

        Task<Cart> GetCartByUserId(int userId);

        Task<CartItem> GetCartItemByProductId(
            int cartId,
            int productId);

        Task<CartItem> GetCartItemByIdForUser(
            int userId,
            int cartItemId);

        void AddCart(Cart cart);

        void AddCartItem(CartItem cartItem);

        void RemoveCartItem(CartItem cartItem);

        Task SaveChanges();

        Task<CartResponseDto> GetCartResponse(int userId);
    }
}