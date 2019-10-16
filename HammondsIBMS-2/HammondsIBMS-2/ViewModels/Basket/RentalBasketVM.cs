using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using HammondsIBMS_2.ViewModels.Accounts;
using HammondsIBMS_Domain.Entities.Accounts;
using HammondsIBMS_Domain.Entities.Contracts;
using HammondsIBMS_Domain.Entities.Stock;

namespace HammondsIBMS_2.ViewModels.Basket
{
    public class RentalBasketVM :RentalBasket
    {

        [DisplayName("Model")]
        public string ModelDescription { get; set; }
       

        [UIHint("Money")]
        [DisplayName("Rent Price")]
        public decimal ModelRentPrice { get; set; }

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
            get { return TotalRental + TotalDeposit + TotalItemCharges; }
        }

        [DisplayName("Total Due")]
        [DataType(DataType.Currency)]
        public decimal TotalToPay
        {
            get { return TotalRental + TotalDeposit + TotalItemCharges; }
        }

        [DisplayName("Total Charges")]
        [DataType(DataType.Currency)]
        public decimal TotalItemCharges
        {
            get { return ItemChargesTotal(); }
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

        [DisplayName("Total Deposit")]
        [DataType(DataType.Currency)]
        public decimal TotalDeposit
        {
            get { return (RentedUnits == null ? 0 : RentedUnits.Sum(c => c.Deposit)); }
        }

        [DisplayName("Total Rental")]
        [DataType(DataType.Currency)]
        public decimal TotalRental
        {
            get { return (RentedUnits == null ? 0 : RentedUnits.Sum(c => c.Amount))*(RentalMonthsDue<=0?1:RentalMonthsDue); }
        }
    }
}