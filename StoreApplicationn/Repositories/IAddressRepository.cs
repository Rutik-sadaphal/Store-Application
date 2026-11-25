using System.Collections.Generic;
using System.Threading.Tasks;

namespace StoreApplicationn.Repositories
{
    public interface IAddressRepository
    {
        Task<Address> GetAddressById(int addressId);
        Task<bool> AddressBelongsToUser(int addressId, int userId);
        Task<List<Address>> GetAddressesByUserId(int userId);

        void AddAddress(Address address);
        void UpdateAddress(Address address);
        void DeleteAddress(Address address);

        Task SaveChanges();
    }
}