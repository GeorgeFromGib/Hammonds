using HammondsIBMS_Data.Infrastructure;
using HammondsIBMS_Domain.Entities.AccountTransactions;
using HammondsIBMS_Domain.Repositories;

namespace HammondsIBMS_Data.Repositories
{
    public interface IAccountTransactionRepository:IRepository<AccountTransaction>
    {
    }

    public class AccountTransactionRepository:RepositoryBase<AccountTransaction>, IAccountTransactionRepository
    {
        public AccountTransactionRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }
}