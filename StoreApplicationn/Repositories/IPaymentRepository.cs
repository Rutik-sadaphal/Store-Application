
using System.Threading.Tasks;

namespace StoreApplicationn.Repositories
{
    public interface IPaymentRepository
    {
        void AddPayment(Payment payment);
        Task SaveChanges();
    }
}
