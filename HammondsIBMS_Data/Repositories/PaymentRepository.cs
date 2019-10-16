using HammondsIBMS_Data.Infrastructure;
using HammondsIBMS_Domain.Entities.Payments;
using HammondsIBMS_Domain.Repositories;

namespace HammondsIBMS_Data.Repositories
{
    public interface IPaymentRepository:IRepository<Payment>
    {
    }

    public class PaymentRepository:RepositoryBase<Payment>, IPaymentRepository
    {
        public PaymentRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }
}