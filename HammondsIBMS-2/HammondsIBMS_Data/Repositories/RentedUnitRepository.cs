using HammondsIBMS_Data.Infrastructure;
using HammondsIBMS_Domain.Entities.Contracts;
using HammondsIBMS_Domain.Repositories;

namespace HammondsIBMS_Data.Repositories
{
    public class RentedUnitRepository:RepositoryBase<RentedUnit>, IRentedUnitRepository
    {
        public RentedUnitRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }
}