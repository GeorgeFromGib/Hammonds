using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HammondsIBMS_2.ViewModels.StockInvoice;
using HammondsIBMS_Domain.Values;

namespace HammondsIBMS_2.ViewModels
{
    public class StockInvoiceItemVM
    {

        public int StockInvoiceItemId { get; set; }

        public bool InvClosed { get; set; }

        public List<SelectableStockVM> StockItems { get; set; }

        //public StockInvoice Invoice { get; set; }

        [DisplayName("Unit Cost")]
        [UIHint("ForeignCurrency")]
        public ForeignCurrency UnitCost { get; set; }

        public string UnitCostDisplay
        {
            get
            {
                return UnitCost==null?"0.00":UnitCost.Symbol + UnitCost.AmountNative.ToString("F") + " (" + UnitCost.AmountGBP.ToString("C2") +
                       ")";
            }
        }

        [DisplayName("Duty Pct.")]
        [UIHint("Percent")]
        public decimal DutyPct { get; set; }

        [DataType(DataType.Currency)]
        [UIHint("Money")]
        public decimal Markup { get; set; }

        [DisplayName("Rental Price")]
        [DataType(DataType.Currency)]
        [UIHint("Money")]
        public decimal RentalPrice { get; set; }

        [DisplayName("Freight+ App.")]
        [DataType(DataType.Currency)]
        public decimal FreightApportionment { get; set; }

        [DisplayName("Landed Cost")]
        [DataType(DataType.Currency)]
        [Editable(false)]
        public decimal LandedCost { get; set; }


        [DisplayName("Units")]
        [Editable(false)]
        public int NoOfUnits { get; set; }


        [DisplayName("Model")]
        public string StockModel { get; set; }

        [ScaffoldColumn(false)]
        public int StockModelId { get; set; }

        [DisplayName("Retail Price")]
        [DataType(DataType.Currency)]
        [UIHint("Money")]
        public decimal RetailPrice { get; set; }

        [DisplayName("Unit Cost Total")]
        [UIHint("ForeignCurrency")]
        [Editable(false)]
        public ForeignCurrency UnitCostTotal { get; set; }

        [DisplayName("Spain RP")]
        [DataType(DataType.Currency)]
        [UIHint("Euro")]
        public decimal SpainRetailPrice { get; set; }

    }
}