using HammondsIBMS_Data.Infrastructure;
using HammondsIBMS_Domain.Entities.Contracts;
using HammondsIBMS_Domain.Repositories;

namespace HammondsIBMS_Data.Repositories
{
    public interface IPaymentPeriodRepository:IRepository<PaymentPeriod>
    {
    }

    public class PaymentPeriodRepository:RepositoryBase<PaymentPeriod>, IPaymentPeriodRepository
    {
        public PaymentPeriodRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }
}