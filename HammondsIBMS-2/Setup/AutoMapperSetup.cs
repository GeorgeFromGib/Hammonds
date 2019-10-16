using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using HammondsIBMS_2.Helpers;
using HammondsIBMS_2.Models;
using HammondsIBMS_2.ViewModels;
using HammondsIBMS_2.ViewModels.Accounts;
using HammondsIBMS_2.ViewModels.Basket;
using HammondsIBMS_2.ViewModels.BillCycleRun;
using HammondsIBMS_2.ViewModels.Contract;
using HammondsIBMS_2.ViewModels.Customer;
using HammondsIBMS_2.ViewModels.DocumentTemplates;
using HammondsIBMS_2.ViewModels.IBMSAccounts;
using HammondsIBMS_2.ViewModels.Invoice;
using HammondsIBMS_2.ViewModels.Manufacturer;
using HammondsIBMS_2.ViewModels.ModelView;
using HammondsIBMS_2.ViewModels.Payment;
using HammondsIBMS_2.ViewModels.StockInvoice;
using HammondsIBMS_2.ViewModels.StockView;
using HammondsIBMS_2.ViewModels.Suppliers;
using HammondsIBMS_Application;
using HammondsIBMS_Domain.Entities.Accounts;
using HammondsIBMS_Domain.Entities.AccountTransactions;
using HammondsIBMS_Domain.Entities.BillCycles;
using HammondsIBMS_Domain.Entities.Contracts;
using HammondsIBMS_Domain.Entities.Customers;
using HammondsIBMS_Domain.Entities.DocumentTemplates;
using HammondsIBMS_Domain.Entities.Invoicing;
using HammondsIBMS_Domain.Entities.Payments;
using HammondsIBMS_Domain.Entities.Stock;
using HammondsIBMS_Domain.Entities.Supplier;
using HammondsIBMS_Domain.Model.Product;
using HammondsIBMS_Domain.Model.Stock;
using HammondsIBMS_Domain.Model.Supplier;


namespace HammondsIBMS_2.Setup
{
    public static class AutoMapperSetup
    {
        public static void Setup()
        {
            Mapper.CreateMap<Model, ModelVM>()
                .ForMember(m => m.Manufacturer, o => o.MapFrom(f => f.Manufacturer.Name))
                .ForMember(m => m.Category, o => o.MapFrom(f => f.Category.Description));

            Mapper.CreateMap<ModelImage, ModelImageEditVM>();
            Mapper.CreateMap<ModelSpecificContract, ModelSpecificContractVM>()
                .ForMember(m => m.ContractDescription, o => o.MapFrom(f => f.ContractType.Description));


            Mapper.CreateMap<StockInvoice, NewStockInvoiceVM>();
            Mapper.CreateMap<StockInvoice, StockInvoiceVM>()
                .ForMember(m=>m.Supplier,o=>o.MapFrom(s=>s.Supplier.Name));
            Mapper.CreateMap<StockInvoice, StockInvoiceChargesVM>();

            Mapper.CreateMap<Stock, SelectableStockVM>()
                .ForMember(s=>s.Model,o=>o.MapFrom(m=>m.Model.ModelCode))
                .ForMember(s=>s.Manufacturer,o=>o.MapFrom(m=>m.Model.Manufacturer.Name))
                .ForMember(s => s.LastEntry,o=>o.MapFrom(s=>s.History.LastOrDefault().TimeStamp));
            Mapper.CreateMap<Stock, StockVM>()
                .ForMember(m => m.ModelCode, o => o.MapFrom(f => f.Model.ModelCode))
                .ForMember(m => m.ManufacturerName, o => o.MapFrom(f => f.Model.Manufacturer.Name))
                .ForMember(m => m.ProductLifeCycle, o => o.MapFrom(f => f.ProductLifeCycleStatus.Description))
                .ForMember(m => m.InvoiceStatus, o => o.MapFrom(f => f.InvoiceStatus.StatusDescription));
            Mapper.CreateMap<Stock, StockEditVM>()
                .ForMember(m => m.ManufacturerAndModel,o => o.MapFrom(f => f.ManufacturerModel));

            Mapper.CreateMap<ExchangeRate, ExchangeRateDropDownVM>();

            Mapper.CreateMap<Manufacturer, ManufacturerVM>();

            Mapper.CreateMap<Supplier, SupplierVM>()
                .ForMember(m=>m.PreferedExchangeRate,o=>o.MapFrom(s=>s.PreferedExchangeRate.Abbreviation+" "+ s.PreferedExchangeRate.RateToGBP));

            Mapper.CreateMap<StockInvoiceItem, StockInvoiceItemVM>();
            Mapper.CreateMap<StockInvoiceItem, StockInvoiceItemsGridViewModel>();
            Mapper.CreateMap<StockInvoiceItemModel, StockInvoiceItemsGridViewModel>();

            Mapper.CreateMap<Invoice, CustomerInvoiceVM>().ForMember(dest=>dest.BillCycle,opt=>opt.MapFrom(f=>f.BillCycle.ToString()));
            Mapper.CreateMap<AccountTransaction, AccountTransactionVM>()
                .ForMember(d => d.AccountTransactionType,o => o.MapFrom(m =>  m.AccountTransactionType.GetDescription()))
                .ForMember(d => d.AccountTransactionInputType,o =>o.MapFrom(m =>  m.AccountTransactionInputType.GetDescription()));

            Mapper.CreateMap<RentedUnit, RentedUnitVM>().ForMember(s => s.StockDescription,
                                                                   o => o.MapFrom(m => m.Stock.ManufacturerModel))
                                                        .ForMember(s=>s.Identifier,o=>o.MapFrom(m=>m.Stock.Identifier))
                                                        .ForMember(s=>s.OldStockProductCycleId,o=>o.MapFrom(m=>m.Stock.ProductLifeCycleId))
                                                        .ForMember(s=>s.OldStockId,o=>o.MapFrom(m=>m.StockId))
                                                        .ForMember(s=>s.OldAmount,o=>o.MapFrom(m=>m.Amount))
                                                        .ForMember(s=>s.ChangedDate,o=>o.UseValue(DateTime.Today));
            Mapper.CreateMap<RentalAccount, EditRentedContractVM>().ForMember(s => s.ContractStatus, o => o.MapFrom(m => m.RentedContract.ContractStatus.Description))
                                                                        .ForMember(s=>s.StartDate,o=>o.MapFrom(m=>m.RentedContract.StartDate))
                                                                        .ForMember(s=>s.ExpiryDate,o=>o.MapFrom(m=>m.RentedContract.ExpiryDate))
                                                                        .ForMember(s=>s.PaymentPeriodId,o=>o.MapFrom(m=>m.RentedContract.PaymentPeriodId))
                                                                        .ForMember(s=>s.PeriodPaymentAmount,o=>o.MapFrom(m=>m.RentedContract.PeriodPaymentAmount))
                                                                        .ForMember(m => m.OldPaymentPeriodId, o => o.MapFrom(s => s.RentedContract.PaymentPeriodId));
            Mapper.CreateMap<PurchaseAccount, AddPurchaseAccountVM>();
            Mapper.CreateMap<PurchaseUnit, PurchasedUnitVM>().ForMember(s => s.StockDescription,
                                                                        o => o.MapFrom(m => m.Stock.ManufacturerModel))
                                                             .ForMember(s=>s.Identifier,o=>o.MapFrom(m=>m.Stock.Identifier));
            Mapper.CreateMap<PurchaseUnit, EditPurchaseUnitVM>();
            Mapper.CreateMap<RentalAccount, AddRentAccountVM>();
            Mapper.CreateMap<RentalContract, EditRentedContractVM>().ForMember(s => s.OldPaymentPeriodId, o => o.MapFrom(m => m.PaymentPeriodId))
                                                                    .ForMember(s => s.ContractStatus, o => o.MapFrom(m => m.ContractStatus.Description))
                                                                    .ForMember(s=>s.PaymentPeriodDescription,o=>o.MapFrom(m=>m.PaymentPeriod.Description))
                                                                    .ForMember(s=>s.MonthlyCharge,o=>o.MapFrom(m=>m.Charge));
            Mapper.CreateMap<ServiceContract, ServiceContractVM>()
                                                .ForMember(s => s.ContractStatusDescription, o => o.MapFrom(m => m.ContractStatus.Description))
                                                .ForMember(s => s.ContractTypeDescription, o => o.MapFrom(m => m.ContractType.Description))
                                                .ForMember(s => s.PaymentPeriodDescription, o => o.MapFrom(m => m.PaymentPeriod.Description))
                                                .ForMember(s=>s.IsScheduled,o=>o.MapFrom(m=>m.ContractStatus.ContractStatusId==(int)ContractStatus.ContractStatuses.Scheduled));
            Mapper.CreateMap<ServiceContract, EditServiceContractVM>()
                                                .ForMember(s => s.ContractTypeDescription,o =>o.MapFrom(m => m.ContractType.Description))
                                                .ForMember(s => s.ContractStatus, o => o.MapFrom(m => m.ContractStatus.Description));
            Mapper.CreateMap<ServiceContract, ServiceContract>();
            Mapper.CreateMap<ServiceContract, ExpireServiceContractVM>().ForMember(s => s.ContractDescription,o =>o.MapFrom(m => m.ContractType.Description))
                                                                        .ForMember(s=>s.ExpiryDate,o=>o.UseValue(DateTime.Today));


            Mapper.CreateMap<RatePerDayAdjustmentPost, PostedVM>()
                                                .ForMember(s => s.InvoiceInfo, o => o.MapFrom(m => m.Invoice != null ? "No. :" + m.Invoice.InvoiceId.ToString() + " " + m.Invoice.BillCycle.ToString() : "Pending"))
                                                //.ForMember(s => s.ContractUnitDescription, o => o.MapFrom(m => m.Contract.CustomerAccount.ContractUnitDescription))
                                                .ForMember(s => s.PostedId, o => o.MapFrom(m => m.AdjustmentPostId))
                                                .ForMember(s => s.Amount, o => o.MapFrom(m=>string.Format("{0:C} per day",m.RatePerDay)));
            Mapper.CreateMap<AmountAdjustmentPost, PostedVM>()
                                                .ForMember(s => s.InvoiceInfo, o => o.MapFrom(m => m.Invoice != null ? "No. :" + m.Invoice.InvoiceId.ToString() + " " + m.Invoice.BillCycle.ToString() : "Pending"))
                                                //.ForMember(s => s.ContractUnitDescription, o => o.MapFrom(m => m.Contract.CustomerAccount.ContractUnitDescription))
                                                .ForMember(s => s.PostedId, o => o.MapFrom(m => m.AdjustmentPostId))
                                                .ForMember(s=>s.Amount,o=>o.MapFrom(m=>string.Format("{0:C}",m.Amount)));
            Mapper.CreateMap<CustomerAccount, AlternateAddressVM>();
            Mapper.CreateMap<CustomerDocument, CustomerDocumentVM>();
            Mapper.CreateMap<Customer, CustomerListVM>()
                .ForMember(m => m.Tel, o => o.MapFrom(s => s.ContactInfo.Tel))
                .ForMember(m => m.Mobile, o => o.MapFrom(s => s.ContactInfo.Mobile))
                .ForMember(m => m.AddressLine1, o => o.MapFrom(s => s.Address.AddressLine1))
                .ForMember(m => m.AddressLine2, o => o.MapFrom(s => s.Address.AddressLine2));
            Mapper.CreateMap<Customer, CustomerBankPaymentDetailsVM>();

            //Mapper.CreateMap<PaymentService.RolledTransaction, RolledTransactionVM>()
            //                                    .ForMember(s=>s.Balance,o=>o.MapFrom(m=>m.Amount))
            //                                    .ForMember(s=>s.BillCycleNo,o=>o.MapFrom(s=>s.BillCycle.BillCycleNo))
            //                                    .ForMember(s=>s.BillCycle,o=>o.MapFrom(m=>m.BillCycle.ToString()));
            Mapper.CreateMap<Payment, PaymentVM>()
                                                .ForMember(s => s.PaymentSource,o => o.MapFrom(s => s.PaymentSource.Description));
            Mapper.CreateMap<PaymentItem, PaymentItemVM>()
                                                //.ForMember(s=>s.AccountDescription,o=>o.MapFrom(m=>m.CustomerAccount.ContractUnitDescription))
                                                .ForMember(s=>s.Type,o=>o.MapFrom(s=>s.PaymentItemType.Description))
                                                .ForMember(s => s.BillCycle,o =>o.MapFrom(m => m.BillCycle.ToString()));
            Mapper.CreateMap<BillCycleRun, BillCycleRunVM>()
                                                .ForMember(s => s.BillCycle,o => o.MapFrom(m => m.BillCycle.ToString()));

            Mapper.CreateMap<Employer, EmployerVM>()
                .ForMember(m=>m.FullAddress,o=>o.MapFrom(p=>p.Address.FullAddress));

            Mapper.CreateMap<DocumentTemplate, DocumentTemplateVM>()
                  .ForMember(m => m.AvailableFields,
                             o =>
                             o.MapFrom(
                                 s =>
                                 s.GetAllowedFieldsWithDelimeters()
                                  .Select(c => new SelectListItem {Text = c, Value = c})));
            Mapper.CreateMap<RentedUnit, DeleteRentedUnitVM>()
                  .ForMember(s => s.StockDescription, o => o.MapFrom(m => m.Stock.ManufacturerModel))
                  .ForMember(s => s.ProductCycleLifeId, o => o.MapFrom(m => m.Stock.ProductLifeCycleId))
                  .ForMember(s => s.RemovalDate, o => o.UseValue(DateTime.Today));

            Mapper.CreateMap<ContractType,ContractTypeVM>()
                .ForMember(s=>s.PaymentPeriodDescription,o=>o.MapFrom(m=>m.DefaultPaymentPeriod.Description));

            Mapper.CreateMap<RentalBasket, RentalBasketVM>();
            Mapper.CreateMap<PurchaseBasket, PurchaseBasketVM>();

        }

        public static IEnumerable<TViewModel> MapList<TEntity, TViewModel>(IEnumerable<TEntity> entityList)
        {
            var map = Mapper.FindTypeMapFor<TEntity, TViewModel>();
            if (map != null)
            {
                return Mapper.Map<IEnumerable<TEntity>, IEnumerable<TViewModel>>(entityList);
            }
            else
            {
                throw new InvalidOperationException("Mapping of '" + typeof(TEntity).Name + "' to '" +
                                                    typeof(TViewModel).Name + "' has not been done");
            }
        }
    }
}