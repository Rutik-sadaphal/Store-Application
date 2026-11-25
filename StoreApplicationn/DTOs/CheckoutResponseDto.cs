namespace StoreApplicationn.DTOs
{
    public class CheckoutResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public int UserId { get; set; }
        public int BillingAddressId { get; set; }
        public int ShippingAddressId { get; set; }

        public int CartItemCount { get; set; }

        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal GrandTotal { get; set; }
    }
}