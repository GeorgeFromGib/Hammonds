using System.Configuration;
using HammondsIBMS_2.Controllers;
using HammondsIBMS_2.Controllers.Account;
using HammondsIBMS_2.Helpers;
using HammondsIBMS_Application;
using HammondsIBMS_Data.Infrastructure;
using HammondsIBMS_Data.Repositories;
using HammondsIBMS_Domain.Repositories;
using ContentType = System.Net.Mime.ContentType;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(HammondsIBMS_2.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(HammondsIBMS_2.App_Start.NinjectWebCommon), "Stop")]

namespace HammondsIBMS_2.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IDatabaseFactory>().To<DatabaseFactory>().InRequestScope();
            kernel.Bind<IUnitOfWork>().To<UnitOfWork>().InRequestScope();
            kernel.Bind<IManufacturerRepository>().To<ManufacturerRepository>();
            kernel.Bind<IModelRepository>().To<ModelRepository>();
            kernel.Bind<IDocumentRepository>().To<DocumentRepository>();
            kernel.Bind<IStockRepository>().To<StockRepository>();
            kernel.Bind<ISupplierRepository>().To<SupplierRepository>();
            kernel.Bind<ISupplierService>().To<SupplierService>();
            kernel.Bind<IStockInvoiceRepository>().To<StockInvoiceRepository>();
            kernel.Bind<IExchangeRateRepository>().To<ExchangeRateRepository>();
            kernel.Bind<IDocumentService>().To<DocumentService>();
            kernel.Bind<IStockInvoiceService>().To<StockInvoiceService>();
            kernel.Bind<ICategoryRepository>().To<CategoryRepository>();
            kernel.Bind<ICustomerRepository>().To<CustomerRepository>();
            kernel.Bind<CustomerService>().To<CustomerService>();
            kernel.Bind<ICustomerAccountRepository>().To<CustomerAccountRepository>();
            kernel.Bind<IContractTypeRepository>().To<ContractTypeRepository>();
            kernel.Bind<ContractService>().To<ContractService>();
            kernel.Bind<IContractRepository>().To<ContractRepository>();
            kernel.Bind<IInvoiceRepository>().To<InvoiceRepository>();
            kernel.Bind<ITransactionRepository>().To<TransactionRepository>();
            kernel.Bind<TransactionsService>().To<TransactionsService>();
            kernel.Bind<IPaymentPeriodRepository>().To<PaymentPeriodRepository>();
            kernel.Bind<IRentedUnitRepository>().To<RentedUnitRepository>();
            kernel.Bind<IPostedRepository>().To<PostedRepository>();
            kernel.Bind<IPaymentSourceRepository>().To<PaymentSourceRepository>();
            kernel.Bind<IPaymentRepository>().To<PaymentRepository>();
            kernel.Bind<IMiscRepository>().To<MiscRepository>();
            kernel.Bind<MiscService>().To<MiscService>();
            kernel.Bind<BillCycleService>().To<BillCycleService>();
            kernel.Bind<IBillCycleRunRepository>().To<BillCycleRunRepository>();
            kernel.Bind<IModelSpecificContractRepository>().To<ModelSpecificContractRepository>();
            kernel.Bind<IItemChargeRepository>().To<ItemChargeRepository>();
            kernel.Bind<ItemChargeService>().To<ItemChargeService>();
            kernel.Bind<IDocumentTemplateRepository>().To<DocumentTemplateRepository>();
            kernel.Bind<IEmployerRepository>().To<EmployerRepository>();
            kernel.Bind<DocumentTemplateService>().To<DocumentTemplateService>();
            kernel.Bind<EmployerService>().To<EmployerService>();
            kernel.Bind<CustomerAccountService>().To<CustomerAccountService>();
            kernel.Bind<TempPurchaseUnitAndContracts>().To<TempPurchaseUnitAndContracts>();
            kernel.Bind<PermPurchaseUnitAndContracts>().To<PermPurchaseUnitAndContracts>();
            kernel.Bind<BasketPurchaseUnitAndContracts>().To<BasketPurchaseUnitAndContracts>();
            kernel.Bind<IAccountTransactionRepository>().To<AccountTransactionRepository>();
            kernel.Bind<AccountTransactionService>().To<AccountTransactionService>();
            kernel.Bind<ContentType>().To<ContentType>();
            kernel.Bind<IBasketRepository>().To<BasketRepository>();
            kernel.Bind<MimeType>().ToMethod(c=>(MimeType)ConfigurationManager.GetSection("mimeTypeMappings"));

        }        
    }
}
