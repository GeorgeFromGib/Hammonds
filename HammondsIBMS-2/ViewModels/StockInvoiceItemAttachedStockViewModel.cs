using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using HammondsIBMS_Domain.Entities.Stock;

namespace HammondsIBMS_2.ViewModels
{
    public class StockInvoiceItemAttachedStockViewModel
    {
        [DisplayName("Select")]
        public bool IsSelected { get; set; }

        public int StockId { get; set; }
      
        public Stock Stock { get; set; }

        public int StockInvoiceItemId { get; set; }

        public StockInvoiceItemAttachedStockViewModel()
        {
            
        }

        public StockInvoiceItemAttachedStockViewModel(Stock stock)
        {
            Stock = stock;
            StockId = stock.StockId;
            IsSelected = false;
        }

        public static IEnumerable<StockInvoiceItemAttachedStockViewModel> CreateList(IEnumerable<Stock> stockList)
        {
            return from s in stockList select new StockInvoiceItemAttachedStockViewModel(s);
        }
    }

}