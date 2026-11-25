using System.Threading.Tasks;
using StoreApplicationn.DTOs;
namespace StoreApplicationn.Repositories
{
    public interface IOrderRepository
    {
        void AddOrder(Order order);
        Task<Order> GetOrderById(int orderId);
        Task SaveChanges();
    }
}