using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using HammondsIBMS_Application;
using HammondsIBMS_Domain.Entities.Accounts;
using HammondsIBMS_Domain.Entities.AccountTransactions;

namespace HammondsIBMS_2.ViewModels.Collections
{
    public class ReceiveCollectionsVM
    {
        public int CustomerId { get; set; }

        [DisplayName("Payment Type")]
        [UIHint("_PaymentSource")]
        public int PreferredPaymentSourceId { get; set; }

        [DisplayName("Reference")]
        public string Reference { get; set; }

        [DisplayName("Current Balance")]
        [DataType(DataType.Currency)]
        public decimal Balance { get; set; }

        public List<CollectionVM> Collections { get; set; }

        //[DisplayName(" Settle In Full")]
        //public bool SettleAllInFull { get; set; }

        //[DataType(DataType.Currency)]
        //[DisplayName("Tendered")]
        //[UIHint("Money")]
        //public decimal TenderedAmount { get; set; }

        //public int CreditedAccountId { get; set; }
        //public decimal CreditedAmount { get; set; }

        //public List<CustomerAccount> CustomerAccounts { get; set; }

        //public bool IsError { get; set; }
        //public string ErrorMessage { get; set; }
    }

  
}