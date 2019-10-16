using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HammondsIBMS_Data.Infrastructure;
using HammondsIBMS_Domain.Entities.Supplier;
using HammondsIBMS_Domain.Model.Supplier;
using HammondsIBMS_Domain.Repositories;

namespace HammondsIBMS_Data.Repositories
{
    public class SupplierRepository:RepositoryBase<Supplier>, ISupplierRepository
    {
        public SupplierRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }
}
