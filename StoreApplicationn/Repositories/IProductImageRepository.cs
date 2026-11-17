
using System.Threading.Tasks;

namespace StoreApplicationn.Repositories
{
    public interface IProductImageRepository
    {
        Task<bool> AddImageIfNotExists(int productId, string imagePath);
    }
}
