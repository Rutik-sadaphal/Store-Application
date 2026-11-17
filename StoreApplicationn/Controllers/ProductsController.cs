
using System;
using System.Threading.Tasks;
using System.Web.Http;
using StoreApplicationn.DTOs;
using StoreApplicationn.Repositories;
using StoreApplicationn.Services;

namespace StoreApplicationn.Controllers
{
    public class ProductsController : ApiController
    {
        private readonly IProductRepository productRepository;
        private readonly IProductImageSyncService imageSyncService;

        public ProductsController()
        {
            productRepository = new ProductRepository();

            imageSyncService = new ProductImageSyncService(
                new ProductImageRepository()
            );
        }

        // GET: api/getProducts
        [HttpGet]
        [Route("api/getProducts")]
        public async Task<IHttpActionResult> GetProducts()
        {
            var products = await productRepository.GetProducts();

            return Ok(products);
        }

        // GET: api/getProduct/{id}
        [HttpGet]
        [Route("api/getProduct/{id}")]
        public async Task<IHttpActionResult> GetProduct(int id)
        {
            var product = await productRepository.GetProductById(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // POST: api/addProduct
        [HttpPost]
        [Route("api/addProduct")]
        public async Task<IHttpActionResult> AddProduct(ProductCreateDto product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int productId = await productRepository.AddProduct(product);

            return Ok(new
            {
                Message = "Product added successfully",
                ProductId = productId
            });
        }

        // PUT: api/updateProduct/{id}
        [HttpPut]
        [Route("api/updateProduct/{id}")]
        public async Task<IHttpActionResult> UpdateProduct(
            int id, ProductUpdateDto product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool isUpdated =
                await productRepository.UpdateProduct(id, product);

            if (!isUpdated)
            {
                return NotFound();
            }

            return Ok("Product updated successfully.");
        }

        // DELETE: api/deleteProduct/{id}
        [HttpDelete]
        [Route("api/deleteProduct/{id}")]
        public async Task<IHttpActionResult> DeleteProduct(int id)
        {
            bool isDeleted =
                await productRepository.DeleteProduct(id);

            if (!isDeleted)
            {
                return NotFound();
            }

            return Ok("Product deleted successfully.");
        }

        // GET: api/searchProducts
        [HttpGet]
        [Route("api/searchProducts")]
        public async Task<IHttpActionResult> SearchProducts(
            string searchText = null,
            int? categoryId = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            string sortBy = "ProductName",
            string sortOrder = "asc",
            int pageNumber = 1,
            int pageSize = 10)
        {
            if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
            {
                return BadRequest(
                    "PageNumber must be at least 1 and PageSize must be between 1 and 100.");
            }

            var products = await productRepository.SearchProducts(
                searchText,
                categoryId,
                minPrice,
                maxPrice,
                sortBy,
                sortOrder,
                pageNumber,
                pageSize);

            return Ok(products);
        }

        // POST: api/syncProductImages
        [HttpPost]
        [Route("api/syncProductImages")]
        public async Task<IHttpActionResult> SyncProductImages()
        {
            try
            {
                int count = await imageSyncService.SyncProductImages();

                return Ok(new
                {
                    Message = "Image synchronization completed successfully.",
                    InsertedCount = count
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
