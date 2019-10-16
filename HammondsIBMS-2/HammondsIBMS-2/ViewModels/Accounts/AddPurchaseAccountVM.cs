using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using HammondsIBMS_Domain.Entities.Accounts;
using HammondsIBMS_Domain.Entities.Contracts;

namespace HammondsIBMS_2.ViewModels.Accounts
{
    public class AddPurchaseAccountVM : PurchaseAccount
    {
        
        //[UIHint("Money")]
        //[DisplayName("Sale Price")]
        //public decimal ModelSalesPrice { get; set; }

        [DisplayName("Start Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }


        public List<SelectItemChargesVM> SelectableItemCharges { get; set; }

        [DisplayName("Payment")]
        [DataType(DataType.Currency)]
        public decimal Payment { get; set; }

        [DataType(DataType.Currency)]
        public decimal Balance
        {
            get { return TotalToPay - Payment; }
        }

        [DisplayName("Total")]
        [DataType(DataType.Currency)]
        public decimal TotalCharge
        {
            get { return TotalUnits + AttachedContractsTotal() + ItemChargesTotal(); }
        }

        [DisplayName("Total Due")]
        [DataType(DataType.Currency)]
        public decimal TotalToPay
        {
            get { return TotalUnits + AttachedContractsTotal() + ItemChargesTotal(); }
        }

        [DisplayName("Total Contracts")]
        [DataType(DataType.Currency)]
        public decimal TotalContracts
        {
            get { return AttachedContractsTotal(); }
        }

        [DisplayName("Total Charges")]
        [DataType(DataType.Currency)]
        public decimal TotalItemCharges
        {
            get { return ItemChargesTotal(); }
        }

        [DisplayName("Total Units")]
        [DataType(DataType.Currency)]
        public decimal TotalUnits
        {
            get { return PurchasedUnits == null ? 0 : PurchasedUnits.Sum(p => p.Total); }
        }

        public  bool HasAlternateAddressOrInstructions
        {
            get { return HasAlternateAddress() || HasInstructions(); }
        }

        private decimal AttachedContractsTotal()
        {
            if (PurchasedUnits == null)
                return 0;
            var tot = PurchasedUnits.Sum(unit => unit.Contracts.Sum(c => c.PeriodPaymentAmount));
            return tot;
        }

        private decimal ItemChargesTotal()
        {
            return (OneOffItems == null ? 0 : OneOffItems.Sum(c => c.TotalCharge));
        }

    }

    public class SelectItemChargesVM
    {
        public int ItemChargeId { get; set; }
        public ItemCharge ItemCharge { get; set; }

        public bool IsSelected { get; set; }
    }
}