using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HammondsIBMS_Data.Infrastructure;
using HammondsIBMS_Domain.Entities.Accounts;
using HammondsIBMS_Domain.Repositories;

namespace HammondsIBMS_Data.Repositories
{
    public interface IBasketRepository : IRepository<Basket>
    {

    }

    public class BasketRepository : RepositoryBase<Basket>, IBasketRepository
    {
        public BasketRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
