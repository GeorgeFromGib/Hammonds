using HammondsIBMS_Data.Infrastructure;
using HammondsIBMS_Domain.Entities.DocumentTemplates;
using HammondsIBMS_Domain.Entities.Invoicing;
using HammondsIBMS_Domain.Entities.Misc;
using HammondsIBMS_Domain.Repositories;

namespace HammondsIBMS_Data.Repositories
{
    public interface ITransactionRepository:IRepository<Transaction>
    {
    }

    public class TransactionRepository:RepositoryBase<Transaction>, ITransactionRepository
    {
        public TransactionRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }
}