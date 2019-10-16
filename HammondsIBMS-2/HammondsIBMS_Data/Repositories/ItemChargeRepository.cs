using HammondsIBMS_Data.Infrastructure;
using HammondsIBMS_Domain.Entities.Contracts;
using HammondsIBMS_Domain.Repositories;

namespace HammondsIBMS_Data.Repositories
{
    public interface IItemChargeRepository:IRepository<ItemCharge>
    {
    }

    public class ItemChargeRepository:RepositoryBase<ItemCharge>, IItemChargeRepository
    {
        public ItemChargeRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }
}