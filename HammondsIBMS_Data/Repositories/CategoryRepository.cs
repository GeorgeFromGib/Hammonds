using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HammondsIBMS_Data.Infrastructure;
using HammondsIBMS_Domain.Entities.Stock;
using HammondsIBMS_Domain.Repositories;

namespace HammondsIBMS_Data.Repositories
{
    public interface ICategoryRepository:IRepository<Category>
    {
    }

    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        public CategoryRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }
}
