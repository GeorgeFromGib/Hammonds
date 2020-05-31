using HammondsIBMS_Domain.Repositories;

namespace HammondsIBMS_Data.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDatabaseFactory databaseFactory;
        private HammondsIBMSContext dataContext;

        public UnitOfWork(IDatabaseFactory databaseFactory)
        {
            this.databaseFactory = databaseFactory;
        }

        protected HammondsIBMSContext DataContext => dataContext ?? (dataContext = databaseFactory.Get());

        public void Commit()
        {
            DataContext.SaveChanges();
        }
    }
}
