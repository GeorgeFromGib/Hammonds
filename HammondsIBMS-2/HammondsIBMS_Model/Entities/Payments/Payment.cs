using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using HammondsIBMS_Domain.Entities.Accounts;
using HammondsIBMS_Domain.Entities.Customers;
using HammondsIBMS_Domain.Entities.Invoicing;

namespace HammondsIBMS_Domain.Entities.Payments
{
    public class Payment
    {
        public int PaymentId { get; set; }

        public DateTime PaymentDate { get; set; }

        public int PaymentSourceId { get; set; }
        public virtual PaymentSource PaymentSource { get; set; }

        public decimal Amount { get; set; }

        public string Reference { get; set; }

        //public int CustomerId { get; set; }
        //public virtual Customer Customer { get; set; }

        public virtual List<PaymentItem> PaymentItems { get; set; } 


    }

    public  class PaymentItem
    {
        public int PaymentItemId { get; set; }

        public decimal Amount { get; set; }

        public BillCycle BillCycle { get; set; }

        //public int CustomerAccountId { get; set; }
        //public virtual CustomerAccount CustomerAccount { get; set; }

        public int PaymentItemTypeId { get; set; }
        public virtual PaymentItemType PaymentItemType { get; set; }

    }

    public class PaymentItemType
    {
        public int PaymentItemTypeId  { get; set; }
        public string Description { get; set; }
     
        public PaymentItemType()
        {
            
        }

        public PaymentItemType(PaymentItemTypes paymentItemTypes)
        {
            PaymentItemTypeId = (int)paymentItemTypes;
        }
    }
    
    public enum PaymentItemTypes
    {
        None=0,
        InvoiceSettlement=1,
        Credit=2
    }

   
}
