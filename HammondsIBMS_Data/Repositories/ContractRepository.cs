using HammondsIBMS_Data.Infrastructure;
using HammondsIBMS_Domain.Entities.Contracts;
using HammondsIBMS_Domain.Repositories;

namespace HammondsIBMS_Data.Repositories
{
    public interface IContractRepository:IRepository<Contract>
    {
    }

    public class ContractRepository:RepositoryBase<Contract>, IContractRepository
    {
        public ContractRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }
}