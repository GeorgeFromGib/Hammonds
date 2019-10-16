using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using HammondsIBMS_Data.Infrastructure;
using HammondsIBMS_Domain.Entities.Stock;
using HammondsIBMS_Domain.Model.Stock;
using HammondsIBMS_Domain.Repositories;

namespace HammondsIBMS_Data.Repositories
{
   

    public class StockRepository:RepositoryBase<Stock>, IStockRepository
    {
        public StockRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }

        public IEnumerable<ProductLifeCycle> AllProductLifeCycles()
        {
            IDbSet<ProductLifeCycle> dbset = DBContext.Set<ProductLifeCycle>();
            return dbset;
        }
    }
}
