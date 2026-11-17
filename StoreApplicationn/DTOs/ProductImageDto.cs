using System;
using System.Collections.Generic;

namespace StoreApplicationn.DTOs
{
    public class ProductImageDto
    {
        public int ProductImageId { get; set; }

        public int ProductId { get; set; }

        public string ImagePath { get; set; }

        public bool IsPrimary { get; set; }

        public DateTime CreatedAt { get; set; }

       
    }
}