
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace StoreApplicationn.Repositories
{
    public class ProductImageRepository : IProductImageRepository
    {
        private readonly ShoppingAppDBEntities db;

        public ProductImageRepository()
        {
            db = new ShoppingAppDBEntities();
        }

        public async Task<bool> AddImageIfNotExists(
            int productId, string imagePath)
        {
            
            var product = await db.Products
                .FirstOrDefaultAsync(p => p.ProductId == productId);

            if (product == null)
                return false;

            
            bool imageExists = await db.ProductImages
                .AnyAsync(i => i.ProductId == productId &&
                               i.ImagePath == imagePath);

            if (imageExists)
                return false;

          
            var image = new ProductImage
            {
                ProductId = productId,
                ImagePath = imagePath,
                IsPrimary = !await db.ProductImages
                    .AnyAsync(i => i.ProductId == productId),
                CreatedAt = DateTime.Now
            };

            db.ProductImages.Add(image);

            await db.SaveChangesAsync();

            return true;
        }
    }
}
