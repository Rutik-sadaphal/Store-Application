using System.ComponentModel.DataAnnotations;

namespace StoreApplicationn.DTOs
{
    public class PaymentRequestDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a valid Order ID.")]
        public int OrderId { get; set; }

        [Required(ErrorMessage = "Please select a payment method.")]
        [RegularExpression("^(UPI|Card|NetBanking|CashOnDelivery)$",
            ErrorMessage = "Invalid payment method.")]
        public string PaymentMethod { get; set; }

        [Required(ErrorMessage = "Please select the payment result.")]
        public bool? SimulateSuccess { get; set; }
    }
}