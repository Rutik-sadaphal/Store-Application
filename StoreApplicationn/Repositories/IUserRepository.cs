
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StoreApplicationn.Repositories
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllUsers();

        Task<User> GetById(int userId);

        Task<User> GetByEmail(string email);

        Task<bool> EmailExists(string email);

        void Add(User user);

        void Delete(User user);

        Task SaveChanges();
    }
}
