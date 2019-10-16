using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HammondsIBMS_Data.Infrastructure;
using HammondsIBMS_Data.Repositories;
using HammondsIBMS_Domain.Entities.Accounts;
using HammondsIBMS_Domain.Entities.Contracts;
using HammondsIBMS_Domain.Entities.Customers;
using HammondsIBMS_Domain.Repositories;

namespace HammondsIBMS_Application
{
    //public interface ICustomerService
    //{
    //    void AddCustomer(Customer customer);
    //    IEnumerable<Customer> GetCustomers();
    //    Customer GetCustomer(int id);
    //    void UpdateCustomer(Customer customer);
    //    void DeleteCustomer (Customer customer);
    //    void CommitChanges();
    //    IEnumerable<ContractUnit> GetContractUnits(int customerId);

    //}

    public class CustomerService:ServiceBase
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ICustomerAccountRepository _contractUnitRepository;
        private readonly IDocumentRepository _documentRepository;

        public CustomerService(ICustomerRepository customerRepository ,ICustomerAccountRepository contractUnitRepository,IDocumentRepository documentRepository ,IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _customerRepository = customerRepository;
            _contractUnitRepository = contractUnitRepository;
            _documentRepository = documentRepository;
        }

        public void AddCustomer(Customer customer)
        {
            ValidateCustomer(customer);
            _customerRepository.Add(customer);
        }

        private static void ValidateCustomer(Customer customer)
        {
            if (string.IsNullOrEmpty(customer.Address.AddressLine1))
                throw new BusinessRuleException("AddressLine1", "AddressLine1 is Required");
        }

        public IEnumerable<Customer> GetCustomers()
        {
            return _customerRepository.GetAll();
        }

        public Customer GetCustomer(int id)
        {
            return _customerRepository.GetById(id);
        }

        public void UpdateCustomer(Customer customer)
        {
            ValidateCustomer(customer);
            _customerRepository.Update(customer);
        }

        public void DeleteCustomer (Customer customer)
        {
            _customerRepository.Delete(customer);
        }

        public IEnumerable<CustomerAccount> GetContractUnits(int customerId)
        {
            return _contractUnitRepository.GetMany(c => c.CustomerId == customerId);
        }

        //public void AddEmployerDetails(Employer mEmployer)
        //{
        //    _customerRepository.AddEmployerDetails(mEmployer);
        //    var cus = GetCustomer(mEmployer.CustomerId);
        //    cus.EmployerId = mEmployer.EmployerId;
        //    UpdateCustomer(cus);
        //}

        public IEnumerable<CustomerAccount> GetCustomerAccounts(int customerId)
        {
            return GetCustomer(customerId).CustomerAccounts;
        }

        public void UpdateEmployerDetails(Employer entity)
        {
            _customerRepository.UpdateEmployerDetails(entity);
        }

        public Employer GetEmployer(int id)
        {
            return _customerRepository.GetEmployerById(id);
        }

        public void DeleteCustomerEmployer(CustomerEmployer customerEmployer)
        {
            _customerRepository.DeleteCustomerEmployer(customerEmployer);
        }

        public void AddCustomerDocument(CustomerDocument cd)
        {
            var cus = _customerRepository.GetById(cd.CustomerId);
            cus.Documents.Add(cd);
            _customerRepository.Update(cus);
        }

        public CustomerDocument GetCustomerDocument(int id)
        {
            return _documentRepository.GetById(id) as CustomerDocument;
        }

        public void DeleteCustomerDocument(CustomerDocument cd)
        {
            _documentRepository.Delete(cd);
        }

        public void UpdateCustomerDocument(CustomerDocument cd)
        {
            _documentRepository.Update(cd);
        }
    }
}
