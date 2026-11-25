using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace StoreApplicationn.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private ShoppingAppDBEntities db = new ShoppingAppDBEntities();

        public void AddOrder(Order order)
        {
            db.Orders.Add(order);
        }

        public async Task<Order> GetOrderById(int orderId)
        {
            return await db.Orders
                .Include(o => o.OrderItems.Select(i => i.Product))
                .Include(o => o.Payments)
                .Include(o => o.OrderAddressSnapshots)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }

        public async Task SaveChanges()
        {
            await db.SaveChangesAsync();
        }
    }
}