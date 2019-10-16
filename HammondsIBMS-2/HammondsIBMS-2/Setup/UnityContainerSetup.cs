using System.Configuration;
using System.Web.Mvc;
using HammondsIBMS_2.Controllers;
using HammondsIBMS_2.Controllers.Account;
using HammondsIBMS_2.Helpers;
using HammondsIBMS_Application;
using HammondsIBMS_Data.Infrastructure;
using HammondsIBMS_Data.Repositories;
using HammondsIBMS_Domain.Entities.Accounts;
using HammondsIBMS_Domain.Repositories;
using Microsoft.Practices.Unity;
using ContentType = System.Net.Mime.ContentType;

namespace HammondsIBMS_2.Setup
{
  public static class UnityContainerSetup
 {
     public static void SetUp()
     {
         var container = new UnityContainer()
             .RegisterType<IDatabaseFactory, DatabaseFactory>(new HttpContextLifetimeManager<IDatabaseFactory>())
             .RegisterType<IUnitOfWork, UnitOfWork>(new HttpContextLifetimeManager<IUnitOfWork>())
             .RegisterType<IManufacturerRepository, ManufacturerRepository>(
                 new HttpContextLifetimeManager<IManufacturerRepository>())
             .RegisterType<IModelRepository, ModelRepository>(new HttpContextLifetimeManager<IModelRepository>())
             .RegisterType<IDocumentRepository, DocumentRepository>(
                 new HttpContextLifetimeManager<IDocumentRepository>())
             .RegisterType<IStockRepository, StockRepository>(new HttpContextLifetimeManager<IStockRepository>())
             .RegisterType<IExchangeRateRepository, ExchangeRateRepository>(
                 new HttpContextLifetimeManager<IExchangeRateRepository>())
             .RegisterType<ISupplierRepository, SupplierRepository>(
                 new HttpContextLifetimeManager<ISupplierRepository>())
             .RegisterType<ISupplierService, SupplierService>(new HttpContextLifetimeManager<ISupplierService>())
             .RegisterType<IStockInvoiceRepository, StockInvoiceRepository>(
                 new HttpContextLifetimeManager<IStockInvoiceRepository>())
             .RegisterType<IDocumentService, DocumentService>(new HttpContextLifetimeManager<IDocumentService>())
             .RegisterType<IStockInvoiceService, StockInvoiceService>(
                 new HttpContextLifetimeManager<IStockInvoiceService>())
             .RegisterType<ICategoryRepository, CategoryRepository>(
                 new HttpContextLifetimeManager<ICategoryRepository>())
             .RegisterType<ICustomerRepository, CustomerRepository>(
                 new HttpContextLifetimeManager<ICustomerRepository>())
             .RegisterType<CustomerService, CustomerService>(new HttpContextLifetimeManager<CustomerService>())
             .RegisterType<ICustomerAccountRepository, CustomerAccountRepository>(
                 new HttpContextLifetimeManager<ICustomerAccountRepository>())
             .RegisterType<IContractTypeRepository, ContractTypeRepository>(
                 new HttpContextLifetimeManager<IContractTypeRepository>())
             .RegisterType<ContractService, ContractService>(new HttpContextLifetimeManager<ContractService>())
             .RegisterType<InvoiceService, InvoiceService>(new HttpContextLifetimeManager<InvoiceService>())
             .RegisterType<IContractRepository, ContractRepository>(
                 new HttpContextLifetimeManager<IContractRepository>())
             .RegisterType<IInvoiceRepository, InvoiceRepository>(new HttpContextLifetimeManager<IInvoiceRepository>())
             .RegisterType<ITransactionRepository, TransactionRepository>(
                 new HttpContextLifetimeManager<ITransactionRepository>())
             .RegisterType<TransactionsService, TransactionsService>(
                 new HttpContextLifetimeManager<TransactionsService>())
             .RegisterType<IPaymentPeriodRepository, PaymentPeriodRepository>(
                 new HttpContextLifetimeManager<IPaymentPeriodRepository>())
             .RegisterType<IRentedUnitRepository, RentedUnitRepository>(
                 new HttpContextLifetimeManager<IRentedUnitRepository>())
             .RegisterType<IPostedRepository, PostedRepository>(new HttpContextLifetimeManager<IPostedRepository>())
             .RegisterType<IPaymentSourceRepository, PaymentSourceRepository>(
                 new HttpContextLifetimeManager<IPaymentSourceRepository>())
             .RegisterType<StockService, StockService>(new HttpContextLifetimeManager<StockService>())
             .RegisterInstance(typeof (MimeSettings), (MimeSettings) ConfigurationManager.GetSection("mimeTypeMappings"))
             //.RegisterType<PaymentService, PaymentService>(new HttpContextLifetimeManager<PaymentService>())
             .RegisterType<IPaymentRepository, PaymentRepository>(new HttpContextLifetimeManager<IPaymentRepository>())
             .RegisterType<IMiscRepository, MiscRepository>(new HttpContextLifetimeManager<IMiscRepository>())
             .RegisterType<MiscService, MiscService>(new HttpContextLifetimeManager<MiscService>())
             .RegisterType<BillCycleService, BillCycleService>(new HttpContextLifetimeManager<BillCycleService>())
             .RegisterType<IBillCycleRunRepository, BillCycleRunRepository>(
                 new HttpContextLifetimeManager<IBillCycleRunRepository>())
             .RegisterType<IModelSpecificContractRepository, ModelSpecificContractRepository>(
                 new HttpContextLifetimeManager<IModelSpecificContractRepository>())
             .RegisterType<IItemChargeRepository, ItemChargeRepository>(
                 new HttpContextLifetimeManager<IItemChargeRepository>())
             .RegisterType<ItemChargeService, ItemChargeService>(new HttpContextLifetimeManager<ItemChargeService>())
             .RegisterType<IDocumentTemplateRepository, DocumentTemplateRepository>(
                 new HttpContextLifetimeManager<IDocumentTemplateRepository>())
             .RegisterType<IEmployerRepository, EmployerRepository>(
                 new HttpContextLifetimeManager<IEmployerRepository>())
             .RegisterType<DocumentTemplateService, DocumentTemplateService>(
                 new HttpContextLifetimeManager<DocumentTemplateService>())
             .RegisterType<EmployerService, EmployerService>(new HttpContextLifetimeManager<EmployerService>())
             .RegisterType<CustomerAccountService, CustomerAccountService>(
                 new HttpContextLifetimeManager<CustomerAccountService>())
             .RegisterType<TempPurchaseUnitAndContracts, TempPurchaseUnitAndContracts>(
                 new HttpContextLifetimeManager<TempPurchaseUnitAndContracts>())
             .RegisterType<PermPurchaseUnitAndContracts, PermPurchaseUnitAndContracts>(
                 new HttpContextLifetimeManager<PermPurchaseUnitAndContracts>())
             .RegisterType<BasketPurchaseUnitAndContracts, BasketPurchaseUnitAndContracts>(
                 new HttpContextLifetimeManager<BasketPurchaseUnitAndContracts>())
             .RegisterType<IAccountTransactionRepository, AccountTransactionRepository>(
                 new HttpContextLifetimeManager<IAccountTransactionRepository>())
             .RegisterType<AccountTransactionService, AccountTransactionService>(
                 new HttpContextLifetimeManager<AccountTransactionService>())
             .RegisterType<ContentType, ContentType>(new HttpContextLifetimeManager<ContentType>())
             .RegisterType<IBasketRepository, BasketRepository>(new HttpContextLifetimeManager<IBasketRepository>());



                 //.RegisterType<IModelService, ModelService>(new HttpContextLifetimeManager<IModelService>())


         //container.RegisterInstance<DbContext>(DataReferenceSetup.Setup());
         //container.RegisterType<IUnitOfWorkFactory, EFUnitOfWorkFactory>()
         //    .RegisterInstance<IUnitOfWorkFactory>(new EFUnitOfWorkFactory(container.Resolve<DbContext>()));
         //container.RegisterInstance<IUnitOfWork>(container.Resolve<IUnitOfWorkFactory>().Create());
         
         //container.RegisterType<StockAdminService, StockAdminService>();
         //container.RegisterType<StockInvoiceService,StockInvoiceService>();
         //container.RegisterType<SupplierService, SupplierService>();
         //container.RegisterType<StockAdminControllerService, StockAdminControllerService>();
         //container.RegisterType<ImageService, ImageService>();


        
        DependencyResolver.SetResolver(new UnityDependencyResolver(container));
     }
 }

}