using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HammondsIBMS_2.ViewModels.Accounts
{
    public class EditPurchaseUnitVM
    {
        public int UnitId { get; set; }

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
        [UIHint("Money")]
        public decimal DiscountedAmount { get; set; }
    }
}