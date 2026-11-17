using System.Threading.Tasks;
using StoreApplicationn.DTOs;

namespace StoreApplicationn.Services
{
    public interface ICartService
    {
        Task<CartResponseDto> GetCart(int userId);

        Task<CartResponseDto> AddToCart(
            int userId,
            CartItemCreateDto request);

        Task<CartResponseDto> UpdateCartItem(
            int userId,
            int cartItemId,
            CartItemUpdateDto request);

        Task<bool> RemoveCartItem(
            int userId,
            int cartItemId);
    }
}