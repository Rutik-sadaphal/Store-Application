
using System;
using System.Collections.Generic;

namespace StoreApplicationn.DTOs
{
    public class ProductResponseDto
    {
        public int ProductId { get; set; }

        public int CategoryId { get; set; }

        public string ProductName { get; set; }

        public string SKU { get; set; }

        public string Description { get; set; }

        public string ProductType { get; set; }

        public decimal Price { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<ProductImageDto> Images { get; set; }
    }
}
