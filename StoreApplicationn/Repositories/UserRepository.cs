
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace StoreApplicationn.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ShoppingAppDBEntities db;

        public UserRepository()
        {
            db = new ShoppingAppDBEntities();
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await db.Users.ToListAsync();
        }

        public async Task<User> GetById(int userId)
        {
            return await db.Users
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task<User> GetByEmail(string email)
        {
            return await db.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> EmailExists(string email)
        {
            return await db.Users
                .AnyAsync(u => u.Email == email);
        }

        public void Add(User user)
        {
            db.Users.Add(user);
        }

        public void Delete(User user)
        {
            db.Users.Remove(user);
        }

        public async Task SaveChanges()
        {
            await db.SaveChangesAsync();
        }
    }
}
