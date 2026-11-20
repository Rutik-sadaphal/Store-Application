
using System.Data.Entity;
using System.Threading.Tasks;

namespace StoreApplicationn.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly ShoppingAppDBEntities db;

        public AddressRepository()
        {
            db = new ShoppingAppDBEntities();
        }

        public async Task<Address> GetAddressById(int addressId)
        {
            return await db.Addresses
                .FirstOrDefaultAsync(a => a.AddressId == addressId);
        }

        public async Task<bool> AddressBelongsToUser(
            int addressId, int userId)
        {
            return await db.Addresses.AnyAsync(a =>
                a.AddressId == addressId &&
                a.UserId == userId);
        }
    }
}
