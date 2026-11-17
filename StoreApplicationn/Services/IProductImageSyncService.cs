using System.Threading.Tasks;

namespace StoreApplicationn.Services
{
    public interface IProductImageSyncService
    {
        Task<int> SyncProductImages();
    }
}