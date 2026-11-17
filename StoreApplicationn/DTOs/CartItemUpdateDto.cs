
using System.ComponentModel.DataAnnotations;

namespace StoreApplicationn.DTOs
{
    public class CartItemUpdateDto
    {
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
