using HammondsIBMS_Data.Infrastructure;
using HammondsIBMS_Domain.Entities.Contracts;
using HammondsIBMS_Domain.Repositories;

namespace HammondsIBMS_Data.Repositories
{

    public interface IContractTypeRepository : IRepository<ContractType>
    {
    }

    public class ContractTypeRepository : RepositoryBase<ContractType>, IContractTypeRepository
    {
        public ContractTypeRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}