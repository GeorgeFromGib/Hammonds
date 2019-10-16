using System.Collections.Generic;
using System.Data.Entity;
using HammondsIBMS_Data.Infrastructure;
using HammondsIBMS_Domain.Entities.Stock;
using HammondsIBMS_Domain.Repositories;

namespace HammondsIBMS_Data.Repositories
{
    public interface IModelRepository:IRepository<Model>
    {
    }

    public class ModelRepository : RepositoryBase<Model>,IModelRepository
    {
        public ModelRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }

    }
}
