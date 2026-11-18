
using System;
using System.Net;
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

     
        [HttpGet]
        [Route("api/getProducts")]
        public async Task<IHttpActionResult> GetProducts()
        {
            var products = await productRepository.GetProducts();

            return Ok(products); 
        }

       
        [HttpGet]
        [Route("api/getProduct/{id}")]
        public async Task<IHttpActionResult> GetProduct(int id)
        {
            if (id <= 0)
                return BadRequest("Product ID must be greater than zero.");

            var product = await productRepository.GetProductById(id);

            if (product == null)
                return NotFound(); 

            return Ok(product);
        }

        
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

            if (minPrice < 0 || maxPrice < 0)
                return BadRequest("Price cannot be negative.");

            if (minPrice.HasValue && maxPrice.HasValue &&
                minPrice.Value > maxPrice.Value)
            {
                return BadRequest("Minimum price cannot be greater than maximum price.");
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
            catch (Exception)
            {
               
                return InternalServerError(); 
            }
        }
    }
}
