using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using HammondsIBMS_Domain.Entities.Accounts;
using HammondsIBMS_Domain.Entities.AccountTransactions;
using HammondsIBMS_Domain.Entities.BillCycles;
using HammondsIBMS_Domain.Entities.Contracts;
using HammondsIBMS_Domain.Entities.Customers;
using HammondsIBMS_Domain.Entities.DocumentTemplates;
using HammondsIBMS_Domain.Entities.Documents;
using HammondsIBMS_Domain.Entities.Invoicing;
using HammondsIBMS_Domain.Entities.Misc;
using HammondsIBMS_Domain.Entities.Payments;
using HammondsIBMS_Domain.Entities.Stock;
using HammondsIBMS_Domain.Entities.Supplier;
using HammondsIBMS_Domain.Model.Product;
using HammondsIBMS_Domain.Model.Stock;
using HammondsIBMS_Domain.Model.Supplier;

namespace HammondsIBMS_Data
{
    public class HammondsIBMSContext : DbContext
    {
        public DbSet<ExchangeRate> ExchangeRates { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<ProductLifeCycle> ProductLifeCycles { get; set; }
        public DbSet<AccountTypeChangeableLifeCycle> AccountTypeChangeableLifeCycles { get; set; }
        public DbSet<InvoiceStatus> InvoiceStatuses { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<ContractType> ContractTypes { get; set; }
        public DbSet<PaymentPeriod> PaymentPeriods { get; set; }
        public DbSet<TransactionType> AccountEntryTypes { get; set; }
        public DbSet<Employer> Employers { get; set; }
        public DbSet<ModelSpecificContract> ModelSpecificContracts { get; set; }
        public DbSet<OneOffItem> OneOffItems { get; set; }
        public DbSet<OneOffType> OneOffTypes { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<StockInvoice> StockInvoices { get; set; }
        public DbSet<StockInvoiceItem> StockInvoiceItems { get; set; }
        public DbSet<Category> Categories { get; set; } //So far so good in EF 5
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerAccount> ContractUnits { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        //public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Transaction> Accounts { get; set; }
        //public DbSet<AdjustmentPost> Posteds { get; set; }
        public DbSet<PaymentSource> PaymentSources { get; set; }
        public DbSet<PaymentItemType> PaymentItemTypes { get; set; }
        public DbSet<Misc> Miscs { get; set; }
        public DbSet<BillCycleRun> BillCycleRuns { get; set; }
        public DbSet<ItemCharge> ItemCharges { get; set; }
        public DbSet<DocumentTemplate> DocumentTemplates { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<AccountTransaction> AccountTransactions { get; set; }
        public DbSet<Basket> Baskets { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StockInvoice>().HasRequired(r => r.Supplier).WithMany().HasForeignKey(f => f.SupplierId).WillCascadeOnDelete(false);
            modelBuilder.Entity<ServiceContract>().HasRequired(r => r.ContractType).WithMany().HasForeignKey(f => f.ContractTypeId).WillCascadeOnDelete(false);
            //modelBuilder.Entity<Supplier>().HasRequired(p => p.PreferedExchangeRate).WithMany().HasForeignKey(
            //    u => u.ExchangeRateId).WillCascadeOnDelete(false);
            
            //modelBuilder.Entity<ServiceContract>().HasRequired(i=>i.ContractType).WithMany().HasForeignKey(i=>i.ContractTypeId).WillCascadeOnDelete(false);
            //modelBuilder.Entity<AdjustmentPost>().HasOptional(o => o.Invoice).WithMany().HasForeignKey(f => f.InvoiceId);
            //modelBuilder.Entity<Model>().HasOptional(o => o.Image).WithMany().HasForeignKey(f => f.ImageId);
            //modelBuilder.Entity<Payment>().HasRequired(p => p.PaymentSource).WithMany().HasForeignKey(u => u.PaymentSourceId).WillCascadeOnDelete(false);
            //modelBuilder.Entity<Stock>().HasOptional(o => o.CustomerAccount).WithMany().HasForeignKey(u => u.CustomerAccountId).WillCascadeOnDelete(false);
            //modelBuilder.Entity<ContractType>().HasRequired(o => o.DefaultPaymentPeriod).WithMany().HasForeignKey(u => u.PaymentPeriodId).WillCascadeOnDelete(false);

            //modelBuilder.Entity<PurchaseUnit>().ToTable("PurchaseUnit");
            //modelBuilder.Entity<RentedUnit>().ToTable("RentedUnit");
         

        }
    }

}

//base.OnModelCreating(modelBuilder);
//modelBuilder.Entity<ServiceContract>().HasRequired(p => p.CustomerAccount).WithMany().HasForeignKey(u => u.CustomerAccountId).WillCascadeOnDelete(false);
//modelBuilder.Entity<Rental>().HasRequired(p => p.ContractUnitRental).WithOptional(r=>r.Rental); //works
//modelBuilder.Entity<ContractUnitPurchase>().HasRequired(p => p.Stock).WithOptional(o => o.ContractUnit);
// modelBuilder.Entity<ContractUnitPurchase>().HasRequired(p=>p.Stock).WithMany().HasForeignKey(u=>u.StockId).WillCascadeOnDelete(false);
//modelBuilder.Entity<Employer>().HasRequired(p=>p.Customer).WithMany().HasForeignKey(u=>u.CustomerId).WillCascadeOnDelete(false);
//modelBuilder.Entity<Employer>().HasRequired(p => p.Customer).WithOptional(o => o.Employer);
