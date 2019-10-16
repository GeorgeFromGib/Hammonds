using System;
using System.ComponentModel.DataAnnotations;
using HammondsIBMS_Domain.Entities.Contracts;

namespace HammondsIBMS_Domain.Entities.Invoicing
{
    public abstract class AdjustmentPost
    {
        public int AdjustmentPostId { get; set; }

        public int ContractId { get; set; }
        public virtual Contract Contract { get; set; }

        [DataType(DataType.Date)]
        public DateTime PostedDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EffectiveDate { get; set; }

      
        public string Description { get; set; }

        public int? InvoiceId { get; set; }
        public virtual Invoice Invoice { get; set; }

        public bool HasBeenInvoiced
        {
            get { return (InvoiceId != null); }
        }

        public string InvoiceString {
            get { return string.Format("{2}. Posted {0:d} : Effective From : {1:d} ", PostedDate, EffectiveDate, Description); }
        }

        public abstract decimal GetInvoicedAmount(BillCycle billcycle);

    }

    public class RatePerDayAdjustmentPost:AdjustmentPost
    {
        [DataType(DataType.Currency)]
        public decimal RatePerDay { get; set; }

        public override decimal GetInvoicedAmount(BillCycle billCycle)
        {
            return RatePerDay*(billCycle.Date - EffectiveDate).Days;
        }
    }

    public class AmountAdjustmentPost:AdjustmentPost
    {
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        public override decimal GetInvoicedAmount(BillCycle billcycle)
        {
            return Amount;
        }
    }
}