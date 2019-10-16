using HammondsIBMS_Data.Infrastructure;
using HammondsIBMS_Domain.Entities.Stock;
using HammondsIBMS_Domain.Repositories;

namespace HammondsIBMS_Data.Repositories
{
    public interface IModelSpecificContractRepository:IRepository<ModelSpecificContract>
    {
    }

    public class ModelSpecificContractRepository:RepositoryBase<ModelSpecificContract>, IModelSpecificContractRepository
    {
        public ModelSpecificContractRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }
}