using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Data;
using System.Linq.Expressions;
using EntityState = System.Data.Entity.EntityState;

namespace HammondsIBMS_Data.Infrastructure
{
    public abstract class RepositoryDBContext<T> where T : class
    {
        protected HammondsIBMSContext dataContext;
        protected IDbSet<T> Dbset { get; private set; }

        protected RepositoryDBContext(IDatabaseFactory databaseFactory)
        {
            DatabaseFactory = databaseFactory;
            Dbset = DataContext.Set<T>();
            
        }

        protected IDatabaseFactory DatabaseFactory
        {
            get; private set;
        }

        protected HammondsIBMSContext DataContext
        {
            get { return dataContext ?? (dataContext = DatabaseFactory.Get()); }
        }
    }

    public abstract class RepositoryBase<T> : RepositoryDBContext<T> where T : class
    {
        protected RepositoryBase(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }

        public void Refresh(T entity)
        {
            DBContext.Entry<T>(entity).Reload();
        }

        public virtual void Add(T entity)
        {
            Dbset.Add(entity);           
        }

        public virtual void Update(T entity)
        {
            //DBContext.Entry(entity).State = EntityState.Modified;
            //Dbset.Attach(entity);
            //dataContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(T entity)
        {
            Dbset.Remove(entity);           
        }

        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> objects = Dbset.Where<T>(where).AsEnumerable();
            foreach (T obj in objects)
                Dbset.Remove(obj);
        } 
        public virtual T GetById(long id)
        {
            return Dbset.Find(id);
        }

        public virtual T GetById(string id)
        {
            return Dbset.Find(id);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return Dbset;
        }

        public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return Dbset.Where(where);
        }

        public T Get(Expression<Func<T, bool>> where)
        {
            return Dbset.Where(where).FirstOrDefault<T>();
        }

        protected  HammondsIBMSContext DBContext
        {
            get { return DataContext; }
        }

    }
}
