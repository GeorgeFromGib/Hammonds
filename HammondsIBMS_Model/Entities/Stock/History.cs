using System;

namespace HammondsIBMS_Domain.Model.Stock
{
    public class StockHistory
    {
        public int StockHistoryId { get; set; }
        public DateTime TimeStamp { get; set; }
        public int ProductLifeCycleId { get; set; }
        public virtual ProductLifeCycle ProductLifeCycle { get; set; }
        public string User2 { get; set; }

    }
}
