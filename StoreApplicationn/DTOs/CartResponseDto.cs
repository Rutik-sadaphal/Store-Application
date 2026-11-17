using System.Collections.Generic;

namespace StoreApplicationn.DTOs
{
    public class CartResponseDto
    {
        public int CartId { get; set; }

        public int UserId { get; set; }

        public List<CartItemResponseDto> Items { get; set; }

        public decimal Subtotal { get; set; }

        public decimal Tax { get; set; }

        public decimal ShippingCost { get; set; }

        public decimal GrandTotal { get; set; }
    }
}