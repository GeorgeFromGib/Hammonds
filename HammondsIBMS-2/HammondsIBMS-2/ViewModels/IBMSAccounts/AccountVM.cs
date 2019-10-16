using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HammondsIBMS_2.ViewModels.IBMSAccounts
{
    public class AccountTransactionVM
    {
        public int AccountTransactionId { get; set; }

        public string Customer { get; set; }

        public int CustomerAccountId { get; set; }

        public string Note { get; set; }

        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [DataType(DataType.Date)]
        public DateTime TransactionDate { get; set; }

        [DisplayName("Transaction Type")]
        public string AccountTransactionType { get; set; }
        public string AccountTransactionInputType { get; set; }


    }
}