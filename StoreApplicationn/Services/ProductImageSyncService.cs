
using System;
using System.IO;
using System.Threading.Tasks;
using System.Web.Hosting;
using StoreApplicationn.Repositories;

namespace StoreApplicationn.Services
{
    public class ProductImageSyncService : IProductImageSyncService
    {
        private readonly IProductImageRepository _repository;

        public ProductImageSyncService(
            IProductImageRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> SyncProductImages()
        {
            
            string folderPath =
                HostingEnvironment.MapPath("~/Uploads/Products");

           
            if (!Directory.Exists(folderPath))
            {
                throw new DirectoryNotFoundException(
                    "Products folder not found: " + folderPath);
            }

            int count = 0;

            
            foreach (string folder in Directory.GetDirectories(folderPath))
            {
                
                string folderName = Path.GetFileName(folder);

                if (!int.TryParse(folderName, out int productId))
                    continue;

                
                foreach (string file in Directory.GetFiles(folder))
                {
                    string extension = Path.GetExtension(file).ToLower();

                    
                    if (extension == ".jpg" ||
                        extension == ".jpeg" ||
                        extension == ".png")
                    {
                        
                        string applicationPath =
                            HostingEnvironment.MapPath("~/");

                        string imagePath = file.Substring(
                            applicationPath.Length
                        ).Replace("\\", "/");

                       
                        bool added =
                            await _repository.AddImageIfNotExists(
                                productId, imagePath);

                        if (added)
                        {
                            count++;
                        }
                    }
                }
            }

            
            return count;
        }
    }
}
