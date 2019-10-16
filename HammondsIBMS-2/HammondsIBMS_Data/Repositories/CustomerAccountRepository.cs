using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using HammondsIBMS_Data.Infrastructure;
using HammondsIBMS_Domain.Entities.Accounts;
using HammondsIBMS_Domain.Entities.Contracts;
using HammondsIBMS_Domain.Entities.Stock;
using HammondsIBMS_Domain.Repositories;
using System.Linq;

namespace HammondsIBMS_Data.Repositories
{
    public interface ICustomerAccountRepository:IRepository<CustomerAccount>
    {
        IEnumerable<AccountTypeChangeableLifeCycle> GetChangeableLifeCycles();
        IEnumerable<Unit> GetUnits();

        void UpdateUnit(Unit unit);
        void DeleteUnit(Unit unit);
        OneOffItem GeOneOffItem(int id);
    }

    public class CustomerAccountRepository:RepositoryBase<CustomerAccount>,ICustomerAccountRepository
    {
        public CustomerAccountRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }

   
        public IEnumerable<AccountTypeChangeableLifeCycle> GetChangeableLifeCycles()
        {
            return DataContext.AccountTypeChangeableLifeCycles;
        }

        public IEnumerable<Unit> GetUnits()
        {
            return DataContext.Units;
        }

        public void UpdateUnit(Unit unit)
        {

        }

        public void DeleteUnit(Unit unit)
        {
            DataContext.Units.Remove(unit);
        }

        public OneOffItem GeOneOffItem(int id)
        {
            return DataContext.OneOffItems.Find(id);
        }
     

    }
}