using System.Collections.Generic;

namespace HammondsIBMS_2.ViewModels.StockInvoice
{
    public class StockInvoiceItemAttachedStockVM
    {
        public int StockInvoiceItemId { get; set; }
        public bool IsOpen { get; set; }

        public List<SelectableStockVM> SelectableStock { get; set; }

    }
}