using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using HammondsIBMS_Domain.Entities.Stock;
using HammondsIBMS_Domain.Values;

namespace HammondsIBMS_Domain.Model.Stock
{
    public class StockInvoiceItem
    {

        public int StockInvoiceItemId { get; set; }

        public virtual List<Entities.Stock.Stock> StockItems { get; set; }

        public virtual StockInvoice Invoice { get; set; }

        public StockInvoiceItem()
        {
            UnitCost=new ForeignCurrency();
        }

        public ForeignCurrency UnitCost 
        { 
            get { return _unitCost; }
            set { 
                _unitCost = value;
                FreightApportionment = CalculateFreightApportionment();
            }
        }
        private ForeignCurrency _unitCost;


        public decimal DutyPct { get; set; }


        public decimal RentalPrice { get; set; }


        public decimal FreightApportionment { get; set; }

        public decimal LandedCost
        {
            get { return (UnitCost.AmountGBP + FreightApportionment)*(1m + DutyPct/100m); }
        }


        public int NoOfUnits
        {
            get { return this.StockItems==null? 0: this.StockItems.Count; }
        }


        public string StockModel
        {
            get { return (NoOfUnits == 0) ? null : StockItems.Last(s => s.StockId > 0).Model.ModelCode; }
        }


        public int StockModelId
        {
            get { return NoOfUnits==0 ? 0 : StockItems.Last(s => s.StockId > 0).Model.ModelId; }
        }

        public decimal RetailPrice { get; set; }

        public ForeignCurrency UnitCostTotal
        {
            get
            {
                var fc = new ForeignCurrency(UnitCost.AmountNative*NoOfUnits,UnitCost);
                return fc;
            }
        }

        private decimal CalculateFreightApportionment()
        {
            if (Invoice == null)
                return 0;
            Invoice.UpdateItemFreightApportionment();
            return FreightApportionment;

            //var unitCostTotExemptOfThisItem = Invoice.Items.Where(i => i.StockInvoiceItemId != this.StockInvoiceItemId)
            //    .Sum(i => i.UnitCostTotal.AmountGBP);
            //try
            //{
            //    var freightRatio = (Invoice.TotalCharges) / (unitCostTotExemptOfThisItem + this.UnitCostTotal.AmountGBP);
            //    return freightRatio * UnitCost.AmountGBP;
            //}
            //catch (DivideByZeroException)
            //{
            //    return 0;
            //}

        }

        public decimal SpainRetailPrice { get; set; }

        public void RemoveStock(int stockId)
        {
            var str = StockItems.Single(i => i.StockId == stockId);
            str.InvoiceStatusId = (int)InvoiceStatusEnum.Pending;
            StockItems.Remove(str);
            FreightApportionment = CalculateFreightApportionment();
            if (StockItems.Count == 0)
                Invoice.Items.Remove(this);
        }
    }
}

