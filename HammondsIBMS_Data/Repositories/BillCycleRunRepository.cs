using HammondsIBMS_Data.Infrastructure;
using HammondsIBMS_Domain.Entities.BillCycles;
using HammondsIBMS_Domain.Repositories;

namespace HammondsIBMS_Data.Repositories
{
    public interface IBillCycleRunRepository:IRepository<BillCycleRun>
    {
    }

    public class BillCycleRunRepository:RepositoryBase<BillCycleRun>, IBillCycleRunRepository
    {
        public BillCycleRunRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }
}