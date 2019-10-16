using System;
using System.Collections.Generic;
using System.Linq;
using HammondsIBMS_Data.Repositories;
using HammondsIBMS_Domain.Entities.Accounts;
using HammondsIBMS_Domain.Entities.AccountTransactions;
using HammondsIBMS_Domain.Entities.Invoicing;
using HammondsIBMS_Domain.Repositories;
using HammondsIBMS_Domain.Values;

namespace HammondsIBMS_Application
{
    public class AccountTransactionService : ServiceBase
    {
        private readonly PaymentSourceRepository _paymentSourceRepository;
        private readonly ICustomerAccountRepository _customerAccountRepository;
        private readonly IAccountTransactionRepository _transactionRepository;

        public AccountTransactionService(IUnitOfWork unitOfWork, IAccountTransactionRepository transactionRepository,
            PaymentSourceRepository paymentSourceRepository,ICustomerAccountRepository customerAccountRepository) : base(unitOfWork)
        {
            _transactionRepository = transactionRepository;
            _paymentSourceRepository = paymentSourceRepository;
            _customerAccountRepository = customerAccountRepository;
        }

        public void AddAccountTransaction(AccountTransaction accountTransaction, bool saveIfZeroAmount = false)
        {
            if (accountTransaction.Amount != 0 || saveIfZeroAmount)
            {
                _transactionRepository.Add(accountTransaction);
                UpdatePaidUntilIfRental(accountTransaction);
            }
        }

        private void UpdatePaidUntilIfRental(AccountTransaction accountTransaction)
        {
            if (accountTransaction.AccountTransactionType == AccountTransactionType.Rental &&
                (accountTransaction.AccountTransactionInputType == AccountTransactionInputType.Payment || accountTransaction.AccountTransactionInputType == AccountTransactionInputType.Adjustment))
            {
                var account = _customerAccountRepository.GetById(accountTransaction.CustomerAccountId) as RentalAccount;
                var unit = account.RentedUnits.Single(c => c.StockId == accountTransaction.GroupId && c.RemovalDate==null);

                unit.PaidUntil =
                    (unit.PaidUntil == null ? unit.RentedDate : (DateTime) unit.PaidUntil).AddDays(
                        (double) (-accountTransaction.Amount/unit.Amount)*ConstantValues.AvgDaysPerMonth);
            }
        }

        public void AddAccountTransaction(int customerAccount, AccountTransactionType accountTransactionType,
            decimal amount, DateTime transactionTime, AccountTransactionInputType accountTransactionInputType,
            int groupingId = 0, string note = "", bool saveIfZeroAmount = false)
        {
            AddAccountTransaction(new AccountTransaction
            {
                CustomerAccountId = customerAccount,
                AccountTransactionType = accountTransactionType,
                AccountTransactionInputType = accountTransactionInputType,
                Amount = amount,
                TransactionDate = transactionTime,
                GroupId = groupingId,
                Note = note
            }, saveIfZeroAmount);
        }

        public void AddAccountTransaction(int customerAccount, AccountTransactionType accountTransactionType,
            decimal amount, AccountTransactionInputType accountTransactionInputType, int groupingId=0, string note="")
        {
            AddAccountTransaction(customerAccount, accountTransactionType, amount, DateTime.Now,
                accountTransactionInputType, groupingId, note);
        }

        public void AddAccountChargeTransaction(int customerAccount, AccountTransactionType accountTransactionType,
            decimal amount, DateTime transactionDateTime, bool saveIfZeroAmount = false)
        {
            if (amount != 0 || saveIfZeroAmount)
            {
                AddAccountTransaction(new AccountTransaction
                {
                    CustomerAccountId = customerAccount,
                    AccountTransactionType = accountTransactionType,
                    Amount = amount,
                    AccountTransactionInputType = AccountTransactionInputType.Charge,
                    TransactionDate = transactionDateTime
                });
            }
        }

        public void AddAccountChargeTransaction(int customerAccount, AccountTransactionType accountTransactionType,
            decimal amount)
        {
            AddAccountChargeTransaction(customerAccount, accountTransactionType, amount, DateTime.Now);
        }

        public void AddAccountPaymentTransaction(int customerAccount, AccountTransactionType accountTransactionType,
            decimal amount, DateTime transactionDateTime)
        {
            AddAccountTransaction(new AccountTransaction
            {
                CustomerAccountId = customerAccount,
                AccountTransactionType = accountTransactionType,
                Amount = -amount,
                AccountTransactionInputType = AccountTransactionInputType.Payment,
                TransactionDate = transactionDateTime
            });
        }

        public void AddAccountPaymentTransaction(int customerAccount, AccountTransactionType accountTransactionType,
            decimal amount)
        {
            AddAccountPaymentTransaction(customerAccount, accountTransactionType, amount, DateTime.Now);
        }

        public IEnumerable<AccountTransaction> GetTransactionsForAccount(int accountId)
        {
            return _transactionRepository.GetMany(a => a.CustomerAccountId == accountId);
        }

        public IEnumerable<AccountTransaction> GetTransactionsForCustomer(int id)
        {
            return _transactionRepository.GetMany(a => a.CustomerAccount.CustomerId == id);

        }

        public IEnumerable<RolledTransaction> GetOutstandingTransactionsForAccount(int accountId)
        {
            return GetTransactionsForAccount(accountId)
                .GroupBy(g => new {g.CustomerAccountId, g.AccountTransactionType,g.GroupId})
                .Select(c => new RolledTransaction
                {
                    CustomerAccountId = c.Key.CustomerAccountId,
                    AccountTransactionType = c.Key.AccountTransactionType,
                    Amount = c.Sum(s => s.Amount),
                    GroupId = c.Key.GroupId,
                    Note = c.OrderByDescending(s=>s.GroupId).First().Note
                }
                );
        }

        public IEnumerable<PaymentSource> GetPaymentSources()
        {
            return _paymentSourceRepository.GetAll();
        }

        public class RolledTransaction
        {
            public int CustomerAccountId { get; set; }
            public AccountTransactionType AccountTransactionType { get; set; }
            public decimal Amount { get; set; }
            public int GroupId { get; set; }
            public string Note { get; set; }
        }

        public decimal GetCustomerBalance(int id)
        {
            return _transactionRepository.GetMany(a => a.CustomerAccount.CustomerId == id).Sum(c => c.Amount);
        }
    }
}