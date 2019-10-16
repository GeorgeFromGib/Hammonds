using System;
using HammondsIBMS_Domain.Entities.Accounts;

namespace HammondsIBMS_Domain.Entities.AccountTransactions
{
    public class AccountTransaction
    {
        public int AccountTransactionId { get; set; }

        public int CustomerAccountId { get; set; }
        public virtual CustomerAccount CustomerAccount { get; set; }

        public int GroupId { get; set; }
        public string Note { get; set; }

        public AccountTransactionType AccountTransactionType { get; set; }
        public AccountTransactionInputType AccountTransactionInputType { get; set; }

        public DateTime TransactionDate { get; set; }

        public decimal Amount { get; set; }

        public AccountTransaction()
        {
            TransactionDate = DateTime.Now;
        }

    }

    public enum AccountTransactionInputType
    {
        Charge = 0,
        Payment = 1,
        Adjustment=2,
    }
}