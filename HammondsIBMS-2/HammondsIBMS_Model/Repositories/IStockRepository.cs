using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HammondsIBMS_Domain.Entities.Stock;
using HammondsIBMS_Domain.Model.Stock;

namespace HammondsIBMS_Domain.Repositories
{
    public interface IStockRepository : IRepository<Stock>
    {
        IEnumerable<ProductLifeCycle> AllProductLifeCycles();
    }
}
