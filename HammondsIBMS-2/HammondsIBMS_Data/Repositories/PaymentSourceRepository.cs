using HammondsIBMS_Data.Infrastructure;
using HammondsIBMS_Domain.Entities.Invoicing;
using HammondsIBMS_Domain.Repositories;

namespace HammondsIBMS_Data.Repositories
{
    public interface IPaymentSourceRepository:IRepository<PaymentSource>
    {
    }

    public class PaymentSourceRepository:RepositoryBase<PaymentSource>, IPaymentSourceRepository
    {
        public PaymentSourceRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }
}