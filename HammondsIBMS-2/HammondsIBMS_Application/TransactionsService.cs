using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HammondsIBMS_Data.Infrastructure;
using HammondsIBMS_Data.Repositories;
using HammondsIBMS_Domain.Entities.Invoicing;
using HammondsIBMS_Domain.Repositories;

namespace HammondsIBMS_Application
{
    public class TransactionsService:ServiceBase
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IPaymentSourceRepository _paymentSourceRepository;

        public TransactionsService(IUnitOfWork unitOfWork,ITransactionRepository transactionRepository,IPaymentSourceRepository paymentSourceRepository) : base(unitOfWork)
        {
            _transactionRepository = transactionRepository;
            _paymentSourceRepository = paymentSourceRepository;
        }

        public IEnumerable<Transaction> GetAccountsForCustomer(int id)
        {
            return _transactionRepository.GetMany(a => a.CustomerAccount.CustomerId == id);
        }

        public decimal GetBalanceForCustomer(int id)
        {
            return GetAccountsForCustomer(id).Sum(s => s.Amount);
        }

        public IEnumerable<Transaction> GetAll()
        {
            return _transactionRepository.GetAll();
        }

        public Transaction GetById(int id)
        {
            return _transactionRepository.GetById(id);
        }

        public void Add(Transaction account)
        {
            _transactionRepository.Add(account);
        }

        public void Update(Transaction account)
        {
            _transactionRepository.Update(account);
        }

        public IEnumerable<PaymentSource> GetPaymentSources()
        {
            return _paymentSourceRepository.GetAll();
        }

        public void UpdatePaymentSource(PaymentSource paymentSource)
        {
            _paymentSourceRepository.Update(paymentSource);
        }

        public void AddPaymentSource(PaymentSource paymentSource)
        {
            _paymentSourceRepository.Add(paymentSource);
        }

        public PaymentSource GetPaymentSource(int id)
        {
            return _paymentSourceRepository.GetById(id);
        }


    }
}
