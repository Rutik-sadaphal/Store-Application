
using System.ComponentModel.DataAnnotations;

namespace StoreApplicationn.DTOs
{
    public class CartItemCreateDto
    {
        [Required]
        public int ProductId { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
