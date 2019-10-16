using HammondsIBMS_Data.Infrastructure;
using HammondsIBMS_Domain.Entities.Invoicing;
using HammondsIBMS_Domain.Repositories;

namespace HammondsIBMS_Data.Repositories
{
    public interface IPostedRepository:IRepository<AdjustmentPost>
    {
    }

    public class PostedRepository:RepositoryBase<AdjustmentPost>, IPostedRepository
    {
        public PostedRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }
}