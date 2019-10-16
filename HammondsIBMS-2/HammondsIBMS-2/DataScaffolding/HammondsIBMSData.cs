using System.Data.Entity;
using HammondsIBMS_Domain.Entities.Stock;
using HammondsIBMS_Domain.Entities.Supplier;
using HammondsIBMS_Domain.Model.Product;
using HammondsIBMS_Domain.Model.Stock;
using HammondsIBMS_Domain.Model.Supplier;

namespace HammondsIBMS_2.DataScaffolding
{
    public class HammondsIBMS_DB:DbContext
    {
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductLifeCycle> ProductLifeCycles { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<StockInvoice> StockInvoices { get; set; }
        public DbSet<ExchangeRate> ExchangeRates { get; set;}
        public DbSet<Supplier> Suppliers { get; set; }
       
    }
}