
using System.Threading.Tasks;

namespace StoreApplicationn.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly ShoppingAppDBEntities db = new ShoppingAppDBEntities();

        public void AddPayment(Payment payment)
        {
            db.Payments.Add(payment);
        }

        public async Task SaveChanges()
        {
            await db.SaveChangesAsync();
        }
    }
}
