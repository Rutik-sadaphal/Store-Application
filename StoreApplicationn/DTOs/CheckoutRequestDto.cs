
using System.ComponentModel.DataAnnotations;

namespace StoreApplicationn.DTOs
{
    public class CheckoutRequestDto
    {
        [Range(1, int.MaxValue)]
        public int UserId { get; set; }

        [Range(1, int.MaxValue)]
        public int BillingAddressId { get; set; }

        [Range(1, int.MaxValue)]
        public int ShippingAddressId { get; set; }
    }
}
