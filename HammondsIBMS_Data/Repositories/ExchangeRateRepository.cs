using HammondsIBMS_Data.Infrastructure;
using HammondsIBMS_Domain.Model.Supplier;
using HammondsIBMS_Domain.Repositories;

namespace HammondsIBMS_Data.Repositories
{
    public interface IExchangeRateRepository:IRepository<ExchangeRate>
    {
    }

    public class ExchangeRateRepository:RepositoryBase<ExchangeRate>, IExchangeRateRepository
    {
        public ExchangeRateRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }
}