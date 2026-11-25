
using System;
using System.Threading.Tasks;
using StoreApplicationn.DTOs;
using StoreApplicationn.Repositories;

namespace StoreApplicationn.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository orderRepository;
        private readonly ICartRepository cartRepository;
        private readonly IUserRepository userRepository;
        private readonly IAddressRepository addressRepository;
        private readonly CartCalculationService cartCalculationService;

        public OrderService()
        {
            orderRepository = new OrderRepository();
            cartRepository = new CartRepository();
            userRepository = new UserRepository();
            addressRepository = new AddressRepository();
            cartCalculationService = new CartCalculationService();
        }


        public async Task<OrderResponseDto> CreateOrder(OrderCreateDto dto)
        {
            if (dto == null)
                return new OrderResponseDto
                {
                    Message = "Order request is required."
                };

            var user = await userRepository.GetById(dto.UserId);

            if (user == null)
                return new OrderResponseDto
                {
                    UserId = dto.UserId,
                    Message = "Customer does not exist."
                };

            var cart = await cartRepository.GetCartResponse(dto.UserId);

            if (cart == null || cart.Items == null || cart.Items.Count == 0)
                return new OrderResponseDto
                {
                    UserId = dto.UserId,
                    Message = "Your cart is empty."
                };

            var billingAddress = await addressRepository.GetAddressById(dto.BillingAddressId);

            if (billingAddress == null || billingAddress.UserId != dto.UserId)
                return new OrderResponseDto
                {
                    UserId = dto.UserId,
                    Message = "Invalid billing address."
                };

            var shippingAddress = await addressRepository.GetAddressById(dto.ShippingAddressId);

            if (shippingAddress == null || shippingAddress.UserId != dto.UserId)
                return new OrderResponseDto
                {
                    UserId = dto.UserId,
                    Message = "Invalid shipping address."
                };

            cartCalculationService.Calculate(cart);

            decimal totalAmount = cart.GrandTotal;

            var order = new Order
            {
                UserId = dto.UserId,
                AddressId = dto.ShippingAddressId,
                OrderDate = DateTime.Now,
                OrderStatus = "Pending",
                TotalAmount = totalAmount
            };

            foreach (var item in cart.Items)
            {
                order.OrderItems.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                });
            }

            order.OrderAddressSnapshots.Add(new OrderAddressSnapshot
            {
                AddressType = "Billing",
                AddressLine = billingAddress.AddressLine,
                City = billingAddress.City,
                State = billingAddress.State,
                PostalCode = billingAddress.PostalCode,
                Country = billingAddress.Country
            });

            order.OrderAddressSnapshots.Add(new OrderAddressSnapshot
            {
                AddressType = "Shipping",
                AddressLine = shippingAddress.AddressLine,
                City = shippingAddress.City,
                State = shippingAddress.State,
                PostalCode = shippingAddress.PostalCode,
                Country = shippingAddress.Country
            });

            orderRepository.AddOrder(order);
            await orderRepository.SaveChanges();

            return new OrderResponseDto
            {
                OrderId = order.OrderId,
                UserId = order.UserId,
                OrderStatus = order.OrderStatus,
                TotalAmount = order.TotalAmount,
                Message = "Order created successfully."
            };
        }

        public async Task<OrderDetailsDto> GetOrderDetails(int orderId)
        {
            var order = await orderRepository.GetOrderById(orderId);

            if (order == null)
                return null;

            var items = new System.Collections.Generic.List<OrderItemDetailsDto>();

            decimal subtotal = 0;

            foreach (var item in order.OrderItems)
            {
                decimal lineTotal = item.UnitPrice * item.Quantity;
                subtotal += lineTotal;

                items.Add(new OrderItemDetailsDto
                {
                    ProductId = item.ProductId,
                    ProductName = item.Product?.ProductName,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    LineTotal = lineTotal
                });
            }

            var payments = new System.Collections.Generic.List<PaymentDetailsDto>();

            foreach (var payment in order.Payments)
            {
                payments.Add(new PaymentDetailsDto
                {
                    PaymentId = payment.PaymentId,
                    PaymentMethod = payment.PaymentMethod,
                    PaymentStatus = payment.PaymentStatus,
                    Amount = payment.Amount,
                    TransactionReference = payment.TransactionReference,
                    PaymentDate = payment.PaymentDate
                });
            }

            var addresses = new System.Collections.Generic.List<OrderAddressDetailsDto>();

            foreach (var address in order.OrderAddressSnapshots)
            {
                addresses.Add(new OrderAddressDetailsDto
                {
                    AddressType = address.AddressType,
                    AddressLine = address.AddressLine,
                    City = address.City,
                    State = address.State,
                    PostalCode = address.PostalCode,
                    Country = address.Country
                });
            }

            return new OrderDetailsDto
            {
                OrderId = order.OrderId,
                UserId = order.UserId,
                OrderDate = order.OrderDate,
                OrderStatus = order.OrderStatus,
                Subtotal = subtotal,
                GrandTotal = order.TotalAmount,
                Items = items,
                Payments = payments,
                Addresses = addresses
            };
        }
    }
}
