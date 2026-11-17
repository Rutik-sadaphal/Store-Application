
using System.ComponentModel.DataAnnotations;

namespace StoreApplicationn.DTOs
{
    public class ProductCreateDto
    {
        [Required]
        [StringLength(200)]
        public string ProductName { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(50)]
        public string SKU { get; set; }

        public string Description { get; set; }

        [StringLength(50)]
        public string ProductType { get; set; }

        [Range(typeof(decimal), "0.01", "999999999")]
        public decimal Price { get; set; }

        public bool IsActive { get; set; }
    }
}
