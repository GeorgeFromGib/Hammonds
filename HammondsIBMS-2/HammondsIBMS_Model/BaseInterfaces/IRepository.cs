using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace HammondsIBMS_Domain.BaseInterfaces
{
    public interface IRepository<T> where T:class
    {

        IQueryable<T> GetAll();
        IQueryable<T> GetFiltered(Expression<Func<T, bool>> filter,int pageSize, int pageIndex = 1);

        void Add(T entity);
        void Delete(T entity);
        void Update(T entity);
        T Find(int id);
    }
}