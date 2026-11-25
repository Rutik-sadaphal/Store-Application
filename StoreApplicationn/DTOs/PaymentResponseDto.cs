using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StoreApplicationn.DTOs
{
    public class PaymentResponseDto
    {
        public int PaymentId { get; set; }

        public int OrderId { get; set; }

        public string PaymentMethod { get; set; }

        public string PaymentStatus { get; set; }
        public string TransactionReference { get; set; }

        public DateTime? PaymentDate { get; set; }

    }
}