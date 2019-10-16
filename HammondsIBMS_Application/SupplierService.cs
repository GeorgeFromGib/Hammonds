using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HammondsIBMS_Data.Infrastructure;
using HammondsIBMS_Data.Repositories;
using HammondsIBMS_Domain.Entities.Supplier;
using HammondsIBMS_Domain.Model.Supplier;
using HammondsIBMS_Domain.Repositories;

namespace HammondsIBMS_Application
{
    public interface ISupplierService
    {
        IEnumerable<Supplier> GetAllSuppliers();
        void CommitChanges();

        IEnumerable<ExchangeRate> GetAllExchangeRates();
        ExchangeRate GetExchangeRate(int id);

        void UpdateSupplier(Supplier etu);
        Supplier GetSupplier(int id);
        void AddSupplier(Supplier entity);
    }

    public class SupplierService:ServiceBase, ISupplierService
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly IExchangeRateRepository _exchangeRateRepository;


        public SupplierService(ISupplierRepository supplierRepository,IExchangeRateRepository exchangeRateRepository,   IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _supplierRepository = supplierRepository;
            _exchangeRateRepository = exchangeRateRepository;
        }

        public IEnumerable<Supplier> GetAllSuppliers()
        {
            return _supplierRepository.GetAll();
        }

        public IEnumerable<ExchangeRate> GetAllExchangeRates()
        {
            return _exchangeRateRepository.GetAll();
        }

        public ExchangeRate GetExchangeRate(int id)
        {
            return _exchangeRateRepository.GetById(id);
        }

        public void UpdateSupplier(Supplier etu)
        {
            _supplierRepository.Update(etu);
        }

        public void AddSupplier(Supplier entity)
        {
            _supplierRepository.Add(entity);
        }

        public Supplier GetSupplier(int id)
        {
            return _supplierRepository.GetById(id);
        }

    }
}
