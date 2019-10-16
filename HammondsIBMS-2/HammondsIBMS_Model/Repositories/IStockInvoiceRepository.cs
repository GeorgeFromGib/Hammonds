using HammondsIBMS_Domain.Model.Stock;

namespace HammondsIBMS_Domain.Repositories
{
    public interface IStockInvoiceRepository:IRepository<StockInvoice>
    {
        StockInvoiceItem GetInvoiceItem(int id);
    }
}