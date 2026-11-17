
using System.Collections.Generic;
using System.Threading.Tasks;
using StoreApplicationn.DTOs;

namespace StoreApplicationn.Repositories
{
    public interface IProductRepository
    {
        Task<List<ProductResponseDto>> GetProducts();

        Task<ProductResponseDto> GetProductById(int id);

        Task<int> AddProduct(ProductCreateDto product);

        Task<bool> UpdateProduct(int id, ProductUpdateDto product);

        Task<bool> DeleteProduct(int id);

        Task<ProductPagedResponseDto> SearchProducts(
    string searchText,
    int? categoryId,
    decimal? minPrice,
    decimal? maxPrice,
    string sortBy,
    string sortOrder,
    int pageNumber,
    int pageSize);
    }
}
