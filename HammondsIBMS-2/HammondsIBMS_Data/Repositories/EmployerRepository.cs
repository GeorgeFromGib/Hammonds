using HammondsIBMS_Data.Infrastructure;
using HammondsIBMS_Domain.Entities.Contracts;
using HammondsIBMS_Domain.Repositories;

namespace HammondsIBMS_Data.Repositories
{
    public interface IEmployerRepository:IRepository<Employer>
    {
    }

    public class EmployerRepository:RepositoryBase<Employer>, IEmployerRepository
    {
        public EmployerRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }
}