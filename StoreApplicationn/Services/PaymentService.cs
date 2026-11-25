
using System;
using System.Threading.Tasks;
using StoreApplicationn.DTOs;
using StoreApplicationn.Repositories;

namespace StoreApplicationn.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository paymentRepository;
        private readonly IOrderRepository orderRepository;
        private readonly ICartRepository cartRepository;

        public PaymentService()
        {
            paymentRepository = new PaymentRepository();
            orderRepository = new OrderRepository();

            cartRepository = new CartRepository();
        }

        public async Task<PaymentResponseDto> ProcessPayment(PaymentRequestDto request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (!request.SimulateSuccess.HasValue)
                throw new Exception("Please select the payment result.");

            var order = await orderRepository.GetOrderById(request.OrderId);

            if (order == null)
                throw new Exception("Order does not exist.");

            if (order.OrderStatus != "Pending")
                throw new Exception("This order is not eligible for payment.");





            // Check stock before processing a successful payment.
            if (request.SimulateSuccess.Value)
            {
                foreach (var item in order.OrderItems)
                {
                    if (item.Product == null)
                        throw new Exception("Product details could not be loaded.");

                    if (item.Product.StockQuantity < item.Quantity)
                        throw new Exception(
                            "Insufficient stock for " + item.Product.ProductName);
                }
            }




            decimal amount = order.TotalAmount;

            string paymentStatus = request.SimulateSuccess.Value
                ? "Successful"
                : "Failed";

            string transactionReference =
                "MOCK-PAY-" + Guid.NewGuid().ToString("N").Substring(0, 12).ToUpper();

            var payment = new Payment
            {
                OrderId = order.OrderId,
                PaymentMethod = request.PaymentMethod,
                PaymentStatus = paymentStatus,
                Amount = amount,
                TransactionReference = transactionReference,
                PaymentDate = DateTime.Now
            };

            paymentRepository.AddPayment(payment);
            await paymentRepository.SaveChanges();

            if (paymentStatus == "Successful")
            {
                foreach (var item in order.OrderItems)
                {
                    item.Product.StockQuantity -= item.Quantity;
                }

                order.OrderStatus = "Confirmed";

                await orderRepository.SaveChanges();

                await cartRepository.ClearCart(order.UserId);
            }

            return new PaymentResponseDto
            {
                PaymentId = payment.PaymentId,
                OrderId = payment.OrderId,
                PaymentMethod = payment.PaymentMethod,
                PaymentStatus = payment.PaymentStatus,
                TransactionReference = payment.TransactionReference,
                PaymentDate = payment.PaymentDate
            };
        }
    }
}
