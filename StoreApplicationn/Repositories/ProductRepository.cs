
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using StoreApplicationn.DTOs;

namespace StoreApplicationn.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ShoppingAppDBEntities db;

        public ProductRepository()
        {
            db = new ShoppingAppDBEntities();
        }

        // Converts Product entity into ProductResponseDto.
        // Also includes all images belonging to the product.
        private ProductResponseDto MapProductToDto(Product p)
        {
            return new ProductResponseDto
            {
                ProductId = p.ProductId,
                CategoryId = p.CategoryId,
                ProductName = p.ProductName,
                SKU = p.SKU,
                Description = p.Description,
                ProductType = p.ProductType,
                Price = p.Price,
                IsActive = p.IsActive,
                CreatedAt = p.CreatedAt,

                Images = p.ProductImages
                    .Select(i => new ProductImageDto
                    {
                        ProductImageId = i.ProductImageId,
                        ProductId = i.ProductId,
                        ImagePath = i.ImagePath,
                        IsPrimary = i.IsPrimary,
                        CreatedAt = i.CreatedAt
                    })
                    .ToList()
            };
        }

        // it helps to retrive the all products with there images

        public async Task<List<ProductResponseDto>> GetProducts()
        {
            var products = await db.Products
                .Include(p => p.ProductImages)
                .ToListAsync();

            return products
                .Select(p => MapProductToDto(p))
                .ToList();
        }

      
        public async Task<ProductResponseDto> GetProductById(int id)
        {
            var product = await db.Products
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null)
            {
                return null;
            }

            return MapProductToDto(product);
        }

       
        //public async Task<int> AddProduct(ProductCreateDto product)
        //{
        //    var newProduct = new Product
        //    {
        //        ProductName = product.ProductName,
        //        CategoryId = product.CategoryId,
        //        SKU = product.SKU,
        //        Description = product.Description,
        //        ProductType = product.ProductType,
        //        Price = product.Price,
        //        IsActive = product.IsActive
        //    };

        //    db.Products.Add(newProduct);

        //    await db.SaveChangesAsync();

        //    return newProduct.ProductId;
        //}

       
        //public async Task<bool> UpdateProduct(
        //    int id,
        //    ProductUpdateDto product)
        //{
        //    var existingProduct = await db.Products
        //        .FirstOrDefaultAsync(p => p.ProductId == id);

        //    if (existingProduct == null)
        //    {
        //        return false;
        //    }

        //    existingProduct.ProductName = product.ProductName;
        //    existingProduct.CategoryId = product.CategoryId;
        //    existingProduct.SKU = product.SKU;
        //    existingProduct.Description = product.Description;
        //    existingProduct.ProductType = product.ProductType;
        //    existingProduct.Price = product.Price;
        //    existingProduct.IsActive = product.IsActive;

        //    await db.SaveChangesAsync();

        //    return true;
        //}

       
        //public async Task<bool> DeleteProduct(int id)
        //{
        //    var product = await db.Products
        //        .FirstOrDefaultAsync(p => p.ProductId == id);

        //    if (product == null)
        //    {
        //        return false;
        //    }

        //    db.Products.Remove(product);

        //    await db.SaveChangesAsync();

        //    return true;
        //}

       

      public async Task<ProductPagedResponseDto> SearchProducts(
      string searchText,
      int? categoryId,
      decimal? minPrice,
      decimal? maxPrice,
      string sortBy,
      string sortOrder,
      int pageNumber,
      int pageSize)
        {
            var query = db.Products.AsQueryable();

           
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                query = query.Where(p =>
                    p.ProductName.Contains(searchText) ||
                    p.SKU.Contains(searchText) ||
                    p.Description.Contains(searchText));
            }

           
            if (categoryId.HasValue)
            {
                query = query.Where(p =>
                    p.CategoryId == categoryId.Value);
            }

           
            if (minPrice.HasValue)
            {
                query = query.Where(p =>
                    p.Price >= minPrice.Value);
            }

            
            if (maxPrice.HasValue)
            {
                query = query.Where(p =>
                    p.Price <= maxPrice.Value);
            }

            
            int totalRecords = await query.CountAsync();

           
            sortBy = (sortBy ?? "ProductName").ToLower();
            sortOrder = (sortOrder ?? "asc").ToLower();

            switch (sortBy)
            {
                case "price":
                    query = sortOrder == "desc"
                        ? query.OrderByDescending(p => p.Price)
                        : query.OrderBy(p => p.Price);
                    break;

                case "productname":
                    query = sortOrder == "desc"
                        ? query.OrderByDescending(p => p.ProductName)
                        : query.OrderBy(p => p.ProductName);
                    break;

                default:
                    query = query.OrderBy(p => p.ProductName);
                    break;
            }

          
            var products = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(p => p.ProductImages)
                .ToListAsync();

           
            var productDtos = products
                .Select(p => MapProductToDto(p))
                .ToList();

            
            int totalPages = (int)Math.Ceiling(
                (double)totalRecords / pageSize);

       
            return new ProductPagedResponseDto
            {
                Products = productDtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = totalPages
            };
        
        }

    }
}
