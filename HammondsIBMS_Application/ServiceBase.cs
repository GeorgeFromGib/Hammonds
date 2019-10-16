
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using HammondsIBMS_Data.Infrastructure;
using HammondsIBMS_Domain.Entities.Stock;
using HammondsIBMS_Domain.Repositories;

namespace HammondsIBMS_Application
{
    public class ServiceBase
    {
        protected IUnitOfWork _uow;

        protected ServiceBase(IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
        }

        public void CommitChanges()
        {
            _uow.Commit();
        }


    }
}