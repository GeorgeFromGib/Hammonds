using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using HammondsIBMS_Domain.Entities.Accounts;
using HammondsIBMS_Domain.Entities.Contracts;
using HammondsIBMS_Domain.Entities.Stock;
using HammondsIBMS_Domain.Values;

namespace HammondsIBMS_2.ViewModels.Accounts
{
    public interface IAddAccountVM
    {
        List<OneOffItem> ItemCharges { get; set; }
    }

    public class AddRentAccountVM:RentalAccount
    {
        
        [UIHint("Money")]
        [DisplayName("Rent Price")]
        public decimal ModelRentPrice { get; set; }

        [DisplayName("Start Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }


        public List<SelectItemChargesVM> SelectableItemCharges { get; set; }


        public int StockId { get; set; }
        public Stock Stock { get; set; }

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
            get { return TotalRental + TotalDeposit + TotalItemCharges; }
        }

        [DisplayName("Total Due")]
        [DataType(DataType.Currency)]
        public decimal TotalToPay
        {
            get { return TotalRental+TotalDeposit + TotalItemCharges; }
        }

        [DisplayName("Total Contracts")]
        [DataType(DataType.Currency)]
        public decimal TotalContracts
        {
            get { return AttachedContracts.Sum(c => c.Charge); }
        }

        [DisplayName("Total Charges")]
        [DataType(DataType.Currency)]
        public decimal TotalItemCharges
        {
            get { return ItemChargesTotal(); }
        }

        public bool HasAlternateAddressOrInstructions
        {
            get { return HasAlternateAddress() || HasInstructions(); }
        }


        private decimal AttachedContractsTotal()
        {
            return (AttachedContracts == null ? 0 : AttachedContracts.Sum(c => c.PeriodPaymentAmount));
        }

        private decimal ItemChargesTotal()
        {
            return (OneOffItems == null ? 0 : OneOffItems.Sum(c => c.TotalCharge));
        }

        public int RentalMonthsDue
        {
            get
            {
                var today = DateTime.Today;
                var noOfMonthsDue = (today.Month - StartDate.Month) + 12*(today.Year - StartDate.Year);
                return noOfMonthsDue + 1;
            }
        }

        public override decimal TotalRental
        {
            get { return base.TotalRental*RentalMonthsDue; }
        }
    }
}