using System.Collections.Generic;
using System.Data;
using HammondsIBMS_Data.Infrastructure;
using HammondsIBMS_Domain.Entities.Misc;
using HammondsIBMS_Domain.Repositories;

namespace HammondsIBMS_Data.Repositories
{
    public interface IMiscRepository:IRepository<Misc>
    {
       
    }

    public class MiscRepository:RepositoryBase<Misc>, IMiscRepository
    {
        public MiscRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }
}