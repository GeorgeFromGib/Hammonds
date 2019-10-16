
using System.Data.Entity;
using HammondsIBMS_Data.Infrastructure;
using HammondsIBMS_Domain.Entities.Contracts;
using HammondsIBMS_Domain.Entities.Customers;
using HammondsIBMS_Domain.Repositories;

namespace HammondsIBMS_Data.Repositories
{
    public interface ICustomerRepository:IRepository<Customer>
    {
        void AddEmployerDetails(Employer entity);
        void UpdateEmployerDetails(Employer entity);
        Employer GetEmployerById(int id);

        void DeleteCustomerEmployer(CustomerEmployer customerEmployer);
    }

    public class CustomerRepository:RepositoryBase<Customer>, ICustomerRepository
    {
        private DbSet<Employer> _EmployerDataContext;
        private DbSet<CustomerEmployer> _CustomerEmployerContext;

        public CustomerRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
            _EmployerDataContext = DBContext.Set<Employer>();
            _CustomerEmployerContext = DBContext.Set<CustomerEmployer>();
        }

        public void AddEmployerDetails(Employer entity)
        {
            _EmployerDataContext.Add(entity);
        }

        public void UpdateEmployerDetails(Employer entity)
        {
            _EmployerDataContext.Attach(entity);
            dataContext.Entry(entity).State = EntityState.Modified;
        }

        public Employer GetEmployerById(int id)
        {
            return _EmployerDataContext.Find(id);
        }

        public void DeleteCustomerEmployer(CustomerEmployer customerEmployer)
        {
            _CustomerEmployerContext.Remove(customerEmployer);
        }
    }
}