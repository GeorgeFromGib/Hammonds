using System.Data.Entity;
using HammondsIBMS_Data.Infrastructure;
using HammondsIBMS_Domain.Entities.Invoicing;
using HammondsIBMS_Domain.Repositories;

namespace HammondsIBMS_Data.Repositories
{
    public interface IInvoiceRepository:IRepository<Invoice>
    {
        InvoiceItem GetInvoiceItemById(int id);
    }

    public class InvoiceRepository:RepositoryBase<Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {

        }

        public InvoiceItem GetInvoiceItemById(int id)
        {
            IDbSet<InvoiceItem> dbset = DBContext.Set<InvoiceItem>();
            return dbset.Find(id);
        }
    }
}