using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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

        public async Task<bool> AddressBelongsToUser(int addressId, int userId)
        {
            return await db.Addresses.AnyAsync(a =>
                a.AddressId == addressId &&
                a.UserId == userId);
        }

        public async Task<List<Address>> GetAddressesByUserId(int userId)
        {
            return await db.Addresses
                .Where(a => a.UserId == userId)
                .ToListAsync();
        }

        public void AddAddress(Address address)
        {
            db.Addresses.Add(address);
        }

        public void UpdateAddress(Address address)
        {
            db.Entry(address).State = EntityState.Modified;
        }

        public void DeleteAddress(Address address)
        {
            db.Addresses.Remove(address);
        }

        public async Task SaveChanges()
        {
            await db.SaveChangesAsync();
        }
    }
}