using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using HammondsIBMS_Domain.Entities.Stock;
using HammondsIBMS_Domain.Model.Supplier;
using HammondsIBMS_Domain.Values;

namespace HammondsIBMS_Domain.Model.Stock
{
    public class StockInvoice
    {
        //private int _tempItemsIndex=-1;

        [Key]
        public int StockInvoiceid { get; set; }


        private DateTime? _invoiceDate;

        [Required]
        public DateTime? InvoiceDate
        {
            get
            {
                _invoiceDate = _invoiceDate ?? DateTime.Now;
                return _invoiceDate;
            }
            set { _invoiceDate = value; }
        }

        [Required]
        public string InvoiceRef { get; set; }

        public virtual List<StockInvoiceItem> Items { get;  set; }

        public int SupplierId { get; set; }

        public int ExchangeRateId { get; set;}
        public virtual ExchangeRate ExchangeRate { get; set; }

        public void ChangeExchangeRate(ExchangeRate rate)
        {
            InvoiceCurrency.UpdateDenomination(rate);
            UpdateDenominations(InvoiceCurrency);
            ExchangeRateId = rate.ExchangeRateId;
        }

        public void AddStockToItems(Entities.Stock.Stock stock, decimal defaultDutyPercentage = 12m)
        {
            StockInvoiceItem item = null;
            if (Items == null)
                Items = new List<StockInvoiceItem>();
            else
                item = Items.Find(m => m.StockModelId == stock.Model.ModelId);
            if (item == null)
            {
                item = new StockInvoiceItem
                {
                    Invoice = this,
                    //StockInvoiceItemId = (_tempItemsIndex--),
                    DutyPct = defaultDutyPercentage,
                    StockItems = new List<Entities.Stock.Stock>(),
                    UnitCost = new ForeignCurrency().UpdateDenomination(this.InvoiceCurrency),
                    RetailPrice = stock.Model.RetailPrice,
                    RentalPrice = stock.Model.RentalPrice,
                    SpainRetailPrice = stock.Model.SpainRetailPrice
                };
                Items.Add(item);
            }
            item.StockItems.Add(stock);
            stock.InvoiceStatusId = (int) InvoiceStatusEnum.Attached;
            UpdateItemFreightApportionment();
        }

        public void InsertItemsFromOtherInvoice(StockInvoice stockInvoice)
        {
            Items.Clear();
            foreach (var item in stockInvoice.Items)
            {
                Items.Add(new StockInvoiceItem
                {
                    DutyPct = item.DutyPct,
                    FreightApportionment = item.FreightApportionment,
                    RentalPrice = item.RentalPrice,
                    StockItems = item.StockItems,
                    UnitCost = item.UnitCost
                });
            }
        }

        public void RemoveItem(StockInvoiceItem item)
        {
            Items.Remove(item);
            UpdateItemFreightApportionment();
        }


        public ForeignCurrency Freight
        {
            get
            {
                _freight = _freight ?? new ForeignCurrency();
                return _freight;
            }
            set
            {
                _freight = value;
                UpdateItemFreightApportionment();
            }
        }
        private ForeignCurrency _freight;

        public void UpdateItemFreightApportionment()
        {
            if (Items == null) return;

            var unitCostTot = Items.Sum(i => i.UnitCostTotal.AmountGBP);
            if(unitCostTot==0) return;

            var freightRatio = (TotalCharges)/unitCostTot;
            Items.ForEach(i => i.FreightApportionment = freightRatio*i.UnitCost.AmountGBP);
        }


        private ForeignCurrency _invoiceCurrency;

        public ForeignCurrency InvoiceCurrency
        { 
            get
            {
                _invoiceCurrency = _invoiceCurrency ?? new ForeignCurrency();
                return _invoiceCurrency;
            }
            set
            {
                _invoiceCurrency = value;
                UpdateDenominations(value);
            }
        }

        private void UpdateDenominations(ForeignCurrency value)
        {
            Freight.UpdateDenomination(value);
            if (Items != null)
            {
                foreach (var itm in Items)
                {
                    itm.UnitCost.UpdateDenomination(value);
                }
            }
        }




        //public ExchangeRate InvoiceExchangeRate
        //{
        //    get { return _invoiceCurrency.ExchangeRate; }
        //    set
        //    {
        //        _invoiceExchangeRate = value;
        //        SetInvoiceCurrency(value);
        //    }
        //}

        public decimal NativeCurrencyToGBP
        {
            get { return InvoiceCurrency.ExchangeRate; }
            set 
            { 
                InvoiceCurrency.ExchangeRate = value; 
                UpdateDenominations(InvoiceCurrency); 
            }
        }

        private void SetInvoiceCurrency(ExchangeRate exRate)
        {
            InvoiceCurrency.UpdateDenomination(exRate);
            Freight.UpdateDenomination(exRate);
            if(Items!=null)
                foreach (var itm in Items)
                {
                    itm.UnitCost.UpdateDenomination(exRate);
                }

        }

       
        public decimal OtherCharges 
        { 
            get { return _otherCharges; }
            set 
            { 
                _otherCharges = value; 
                UpdateItemFreightApportionment(); 
            }
        }
        private decimal _otherCharges;
        //private ExchangeRate _invoiceExchangeRate;


        public decimal TotalCharges
        {
            get { return OtherCharges + (Freight==null?0:Freight.AmountGBP); }
        }

    
        public decimal InvoiceTotal
        {
            get { return TotalCharges + (Items==null?0:Items.Select(i => i.UnitCost.AmountGBP*i.NoOfUnits).Sum()); }
        }

        public ForeignCurrency ItemsTotal
        {
            get
            {
                var tot =
                    new ForeignCurrency(Items == null ? 0 : Items.Select(i => i.UnitCost.AmountNative*i.NoOfUnits).Sum(),
                                        this.InvoiceCurrency);
                return tot;
            }
        }

        public virtual Entities.Supplier.Supplier Supplier { get; set; }

        public DateTime? DateProcessed { get; set; }

        public bool IsProcessed { get; set; }

        public bool IsCancelled { get; set; }
    }

   
}
