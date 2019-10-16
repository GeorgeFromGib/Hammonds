using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HammondsIBMS_Data.Infrastructure;
using HammondsIBMS_Domain.Model.Stock;
using HammondsIBMS_Domain.Repositories;

namespace HammondsIBMS_Data.Repositories
{
    public class StockInvoiceRepository:RepositoryBase<StockInvoice>, IStockInvoiceRepository
    {
        public StockInvoiceRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }

        public StockInvoiceItem GetInvoiceItem(int id)
        {
            //TODO UNREM
            //return DBContext.StockInvoiceItems.Find(id);
            return null;
        }
    }
}
