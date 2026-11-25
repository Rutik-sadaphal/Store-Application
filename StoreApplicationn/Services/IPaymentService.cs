
using System.Threading.Tasks;
using StoreApplicationn.DTOs;

namespace StoreApplicationn.Services
{
    public interface IPaymentService
    {
        Task<PaymentResponseDto> ProcessPayment(PaymentRequestDto request);
    }
}
