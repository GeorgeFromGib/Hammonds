using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq.Expressions;
using HammondsIBMS_Data.Infrastructure;
using HammondsIBMS_Domain.Model.Product;
using HammondsIBMS_Domain.Repositories;

namespace HammondsIBMS_Data.Repositories
{
    public interface IManufacturerRepository:IRepository<Manufacturer>
    {
    }

    public class ManufacturerRepository:RepositoryBase<Manufacturer>, IManufacturerRepository
    {
        public ManufacturerRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }

    }
}