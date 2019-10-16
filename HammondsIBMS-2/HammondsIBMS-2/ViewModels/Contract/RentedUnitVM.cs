using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HammondsIBMS_2.ViewModels.Contract
{
    
    public class RentedUnitVM : EditCustomerAccountStockVM
    {

        [DataType(DataType.Currency)]
        [UIHint("Money")]
        public decimal Amount { get; set; }

        public decimal OldAmount { get; set; }
        
        public string Notes { get; set; }

        [DisplayName("Rented Date")]
        [DataType(DataType.Date)]
        public DateTime RentedDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? RemovalDate { get; set; }

        public string StockDescription { get; set; }
        public string Identifier { get; set; }

        [DataType(DataType.Currency)]
        [UIHint("Money")]
        public decimal Deposit { get; set; }

        [DisplayName("Change Date")]
        [DataType(DataType.Date)]
        public DateTime? ChangedDate { get; set; }

        [DisplayName("Paid To")]
        [DataType(DataType.Date)]
        public DateTime? PaidUntil { get; set; }

        public bool CanChangeRentedDate { get; set; }
        public bool CanChangeProductCycle { get; set; }
        public bool CanChangeDeposit { get; set; }
        public bool HideStockSelector { get; set; }
        public bool HideChangeDate { get; set; }

        public static RentedUnitVM CreatetRentedUnitVM(int CustomerAccountId, int stockId, int productLifeCycleId)
        {
            return new RentedUnitVM
            {
                CustomerAccountId = CustomerAccountId,
                StockId = stockId,
                OldStockId = stockId,
                OldStockProductCycleId = productLifeCycleId,
                ChangedDate = DateTime.Today
                
            };
        }

        public RentedUnitVM()
        {
            CanChangeRentedDate = true;
            CanChangeProductCycle = true;
            CanChangeDeposit = true;
        }

    }

    public class PurchasedUnitVM : EditCustomerAccountStockVM
    {
        public int PurchaseUnitId { get; set; }



        [DisplayName("Purchased Date")]
        [DataType(DataType.Date)]
        public DateTime PurchasedDate { get; set; }

        [DisplayName("Retail Cost")]
        [DataType(DataType.Currency)]
        public decimal RetailCost { get; set; }

        [DataType(DataType.Currency)]
        public decimal Total { get; set; }

        [DisplayName("Discounted Amount")]
        [DataType(DataType.Currency)]
        public decimal DiscountedAmount { get; set; }

        public string StockDescription { get; set; }
        public string Identifier { get; set; }

      

    }

}