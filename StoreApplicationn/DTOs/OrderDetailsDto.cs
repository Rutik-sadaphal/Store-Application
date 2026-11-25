using System;
using System.Collections.Generic;

namespace StoreApplicationn.DTOs
{
    public class OrderDetailsDto
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderStatus { get; set; }
        public decimal Subtotal { get; set; }
        public decimal GrandTotal { get; set; }

        public List<OrderItemDetailsDto> Items { get; set; }
        public List<PaymentDetailsDto> Payments { get; set; }
        public List<OrderAddressDetailsDto> Addresses { get; set; }
    }
}