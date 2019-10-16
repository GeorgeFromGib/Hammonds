using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HammondsIBMS_Domain.Values;

namespace HammondsIBMS_2.ViewModels
{
    public class StockInvoiceItemsGridViewModel
    {
        [ScaffoldColumn(false)]
        public int StockInvoiceItemId { get; set; }


        [DisplayName("Unit Cost")]
        public ForeignCurrency UnitCost { get; set; }

        [DisplayName("Duty")]
        [UIHint("Percent")]
        public decimal DutyPct { get; set; }

        [DisplayName("Rental Price")]
        [DataType(DataType.Currency)]
        public decimal RentalPrice { get; set; }

        [DisplayName("Freight+ App.")]
        [DataType(DataType.Currency)]
        public decimal FreightApportionment { get; set; }

        [DisplayName("Landed Cost")]
        [DataType(DataType.Currency)]
        public decimal LandedCost { get; set; }
     
        [DisplayName("Units")]
        public int NoOfUnits { get; private set; }

        [DisplayName("Model")]
        public string StockModel { get; private set; }


        [ScaffoldColumn(false)]
        public int StockModelId { get; set; }


        [DisplayName("Retail Price")]
        [DataType(DataType.Currency)]
        public decimal RetailPrice { get; set; }

        [DisplayName("Spain RP")]
        [DataType(DataType.Currency)]
        [UIHint("Euro")]
        public decimal SpainRetailPrice { get; set; }


        public StockInvoiceItemsGridViewModel()
        {
            UnitCost = new ForeignCurrency();
        }


    }

    public class StockInvoiceItemsEditViewModel
    {
        public bool InvClosed { get; set; }
        public List<StockInvoiceItemsGridViewModel> StockInvoiceItemsGridViewModels { get; set; }
    }
}