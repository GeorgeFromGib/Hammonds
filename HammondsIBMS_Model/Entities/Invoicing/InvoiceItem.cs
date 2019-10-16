using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace HammondsIBMS_Domain.Entities.Invoicing
{
    public abstract class InvoiceItem
    {
        public int InvoiceItemId { get; set; }

        [DataType(DataType.Currency)]
        public abstract decimal Amount { get;  }

    }

    public class CustomerAccountInvoiceItem : InvoiceItem
    {
        public int CustomerAccountId { get; set; }
        public string CustomerAccountDetails { get; set; }
        public virtual List<InvoiceAttachedContractItems> AttachedContractItems { get; set; }

        public override decimal Amount
        {
            get { return AttachedContractItems.Sum(c => c.Amount); }
        }
    }

    public class InvoiceAttachedContractItems
    {
        public int InvoiceAttachedContractItemsId { get; set; }
        public string Description { get; set; }

        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }
    }

    public class InvoicePosted:InvoiceItem
    {
        public int CustomerAccountId { get; set; }
        public string CustomerAccountDetails { get; set; }
        public string Description { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime PostedDate { get; set; }

        public decimal ProRatedAmount { get; set; }

        [DataType(DataType.Currency)]
        public override decimal Amount
        {
            get { return ProRatedAmount; }
        }
    }
}