
using System.Threading.Tasks;

namespace StoreApplicationn.Repositories
{
    public interface IAddressRepository
    {
        Task<Address> GetAddressById(int addressId);

        Task<bool> AddressBelongsToUser(int addressId, int userId);
    }
}

