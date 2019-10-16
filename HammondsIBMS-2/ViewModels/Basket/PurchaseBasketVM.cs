using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using HammondsIBMS_2.ViewModels.Accounts;
using HammondsIBMS_Domain.Entities.Accounts;

namespace HammondsIBMS_2.ViewModels.Basket
{
    public class PurchaseBasketVM:PurchaseBasket
    {

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
            get { return TotalUnits + TotalContracts + ItemChargesTotal(); }
        }

        [DisplayName("Total Due")]
        [DataType(DataType.Currency)]
        public decimal TotalToPay
        {
            get { return TotalUnits + TotalContracts + ItemChargesTotal(); }
        }

        [DisplayName("Total Contracts")]
        [DataType(DataType.Currency)]
        public decimal TotalContracts { get; set; }
      

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


        private decimal ItemChargesTotal()
        {
            return (OneOffItems == null ? 0 : OneOffItems.Sum(c => c.TotalCharge));
        }
    }
}