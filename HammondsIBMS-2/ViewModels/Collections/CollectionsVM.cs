using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc.Html;
using HammondsIBMS_Domain.Entities.AccountTransactions;

namespace HammondsIBMS_2.ViewModels.Collections
{
    public class CollectionVM
    {
        [DisplayName("Account")]
        public int CustomerAccountId { get; set; }


        public string AccountType { get; set; }

        public int AccountTransactionType { get; set; }
        
        [DisplayName("Transaction Type")]
        public string AccountTransactionTypeDescription { get; set; }
        
        [DataType(DataType.Currency)] //float (rather than decimal) required for json Mapping
        public float Amount { get; set; }

        [DataType(DataType.Currency)]
        public float Payment { get; set; }

        public int GroupId { get; set; }

        public string Note { get; set; }

        public string AccountNoAndType
        {
            get { return string.Format("{0} ({1})", CustomerAccountId, AccountType); }
        }
    }
}