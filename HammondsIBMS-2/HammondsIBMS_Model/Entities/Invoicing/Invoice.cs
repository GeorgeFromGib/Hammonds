using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using HammondsIBMS_Domain.Entities.Contracts;
using HammondsIBMS_Domain.Entities.Customers;

namespace HammondsIBMS_Domain.Entities.Invoicing
{
    public class Invoice
    {
        public int InvoiceId { get; set; }
        
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        
        public BillCycle BillCycle { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime InvoiceDate { get; set; }

        public virtual List<InvoiceItem> InvoiceItems { get; set; }

        public virtual List<InvoicePosted> PostedItems { get; set; }

        [DataType(DataType.Currency)]
        public decimal InvoicedTotal
        {
            get { return InvoiceItems.Sum(c => c.Amount)+PostedItems.Sum(c=>c.Amount); }
        }

        [DataType(DataType.Currency)]
        public decimal PreviousBalance { get; set; }

        [DataType(DataType.Currency)]
        public decimal InvoiceBalance
        {
            get { return InvoicedTotal + PreviousBalance; }
        }

    }
}