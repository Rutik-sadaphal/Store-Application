using System.Collections.Generic;

namespace StoreApplicationn.DTOs
{
    public class ProductPagedResponseDto
    {
        public List<ProductResponseDto> Products { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalRecords { get; set; }

        public int TotalPages { get; set; }
    }
}