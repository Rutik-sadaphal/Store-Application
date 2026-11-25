using System;

namespace StoreApplicationn.DTOs
{
    public class PaymentDetailsDto
    {
        public int PaymentId { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }
        public decimal Amount { get; set; }
        public string TransactionReference { get; set; }
        public DateTime? PaymentDate { get; set; }
    }
}