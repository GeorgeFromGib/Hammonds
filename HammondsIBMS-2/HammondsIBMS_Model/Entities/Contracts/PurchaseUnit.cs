using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace HammondsIBMS_Domain.Entities.Contracts
{
    public class PurchaseUnit:Unit
    {
        public int PurchaseUnitId { get; set; }


        [DisplayName("Purchased Date")]
        [DataType(DataType.Date)]
        public DateTime PurchasedDate { get; set; }

        [DisplayName("Retail Cost")]
        [DataType(DataType.Currency)]
        public decimal RetailCost { get; set; }

        [DataType(DataType.Currency)]
        public decimal Total
        {
            get { return RetailCost - DiscountedAmount; }
        }

        [DisplayName("Discounted Amount")]
        [DataType(DataType.Currency)]
        [UIHint("Money")]
        public decimal DiscountedAmount { get; set; }

        public virtual List<ServiceContract> Contracts { get; set; }

        public decimal TotalContracts
        {
            get
            {
                return Contracts == null ? 0 : Contracts.Sum(c => c.PeriodPaymentAmount);
            }
        }

    }
}