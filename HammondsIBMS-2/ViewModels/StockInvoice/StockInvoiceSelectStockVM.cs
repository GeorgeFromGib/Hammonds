using System.Collections.Generic;
using System.ComponentModel;

namespace HammondsIBMS_2.ViewModels.StockInvoice
{
    public class StockInvoiceSelectStockVM
    {
        public int StockId { get; set; }
        public List<SelectableStockVM> SelectableStock { get; set; }
    }
}