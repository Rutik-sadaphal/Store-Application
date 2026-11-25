using System.Threading.Tasks;
using StoreApplicationn.DTOs;

namespace StoreApplicationn.Services
{
    public interface IOrderService
    {
        Task<OrderResponseDto> CreateOrder(OrderCreateDto dto);

        Task<OrderDetailsDto> GetOrderDetails(int orderId);
    }
}