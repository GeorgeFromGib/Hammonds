using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using HammondsIBMS_Data;
using HammondsIBMS_Domain.Entities.Accounts;
using HammondsIBMS_Domain.Entities.Contracts;
using HammondsIBMS_Domain.Entities.Customers;
using HammondsIBMS_Domain.Entities.DocumentTemplates;
using HammondsIBMS_Domain.Entities.Invoicing;
using HammondsIBMS_Domain.Entities.Misc;
using HammondsIBMS_Domain.Entities.Payments;
using HammondsIBMS_Domain.Entities.Stock;
using HammondsIBMS_Domain.Entities.Supplier;
using HammondsIBMS_Domain.Model.Product;
using HammondsIBMS_Domain.Model.Stock;
using HammondsIBMS_Domain.Model.Supplier;
using HammondsIBMS_Domain.Values;
using LumenWorks.Framework.IO.Csv;

namespace HammondsIBMS_2.DataScaffolding
{
    public static class DbPopulator
    {
        const string MigrationFilePath=@"~/DataScaffolding/MigrationFiles";
        const string MigrationDocTemplates = @"~/DataScaffolding/DocumentTemplates";

        public static void ClearTables(HammondsIBMSContext context)
        {
            context.Database.ExecuteSqlCommand("EXEC sp_msforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT all' ");
            context.Database.ExecuteSqlCommand("EXEC sp_MSForEachTable 'DELETE FROM ?' ");
            context.Database.ExecuteSqlCommand("EXEC sp_msforeachtable 'ALTER TABLE ? WITH CHECK CHECK CONSTRAINT all' ");
            try {
            context.Database.ExecuteSqlCommand("EXEC sp_msforeachtable 'DBCC CHECKIDENT(''?'', RESEED, 0)' ");
            }
            catch
            {
            }
        }

        public static void Populate(HammondsIBMSContext context,bool loadTestData=true)
        {

            ExchangeRangeSetup(context);

            var pCycles = ProductLifeCyclesSetup(context);

            AccountTypesChangeableProductLifeCyclesSetup(context, pCycles);

            InvoiceStatusSetup(context);

            CategoriesSetup(context);

            PaymentPeriodsSetup(context);

            ContractTypesSetup(context);

            var paymentSources = PaymentSourcesSetup(context);

            AccountTypesSetup(context);

            OneOfftypesSetup(context);

            PaymentTypesSetup(context);

            MiscSetup(context);

            if (loadTestData)
            {

                SuppliersSetup(context);

                var manufactures = ManufacturersSetup(context);

                ModelsSetup(context, manufactures);

                StockSetup(context);

                ItemChargesSetup(context);

                CustomersSetup(context, paymentSources);

                DocumentTemplatesSetup(context);

                EmployersSetup(context);
            }
        }



        private static void EmployersSetup(HammondsIBMSContext context)
        {
            #region Employers

            var employers = new List<Employer>()
            {
                new Employer {EmployerName = "ACME Ltd", Address = new Address {AddressLine1 = "Mojavi Desert"}}
            };

            employers.ForEach(e => context.Employers.Add(e));

            #endregion
        }

        private static void DocumentTemplatesSetup(HammondsIBMSContext context)
        {
            #region Document Templates

            var docTemplates = new List<DocumentTemplate>
            {
                new PurchaseContractDocumentTemplate
                {
                    DocumentTemplateId = (int) DocumentTemplateTypes.Sale,
                    Title = "Sale",
                    Body = GetDocumentTemplateText("Sale.txt")
                },
                new RentContractDocumentTemplate
                {
                    DocumentTemplateId = (int) DocumentTemplateTypes.Rent,
                    Title = "Rent",
                    Body = GetDocumentTemplateText("Rent.txt")
                },
            };

            docTemplates.ForEach(m => context.DocumentTemplates.Add(m));

            #endregion
        }

        private static void MiscSetup(HammondsIBMSContext context)
        {
            #region Misc

            var miscs = new List<Misc>()
            {
                new Misc
                {
                    MiscId = (int) MiscIdentifier.DefaultDutyPercentage,
                    Identifier = "Default Duty Percentage",
                    Value = "12",
                    ValueRegexFilter = @"^[0-9]+(?:\.[0-9]*)?$",
                    FormatExample = "12, 12.5, 10.12",
                    CanEdit = true
                },
                new Misc
                {
                    MiscId = (int) MiscIdentifier.LastBillCycle,
                    Identifier = "Last Bill Cycle",
                    Value = "2012-02",
                    ValueRegexFilter = @"^2[0-9]{3}-(0[1-9]|1[0-2])$",
                    FormatExample = "2012-04 [format : yyyy-mm]",
                    CanEdit = true
                },
                new Misc
                {
                    MiscId = (int) MiscIdentifier.MinPeriodForBillCycles,
                    Identifier = "Minimum period(days) between Bill Cycles",
                    Value = "27",
                    ValueRegexFilter = @"^([0-9]{1}|[1-2][0-9]|3[0-2])$",
                    FormatExample = "27 [Range : 0-33]",
                    CanEdit = true
                },
                new Misc
                {
                    MiscId = (int) MiscIdentifier.ContractExpiryWarningPeriod,
                    Identifier = "Contract Expiry Warning (days before)",
                    Value = "30",
                    ValueRegexFilter = @"^([1-9]{1}|[1-2][0-9]{2})$",
                    FormatExample = "27 [Range : 1-299]",
                    CanEdit = true
                },
                new Misc
                {
                    MiscId = (int) MiscIdentifier.DefaultDepositPerUnit,
                    Identifier = "Default deposit per rental unit",
                    Value = "50",
                    ValueRegexFilter = @"^([0-9]{1}|[1][0-9]{1,3})$",
                    FormatExample = "55 [Range : 0-1999]",
                    CanEdit = true
                },
                 new Misc
                {
                    MiscId = (int) MiscIdentifier.LastHouseKeep,
                    Identifier = "Last House Keep Date",
                    Value = "1/1/1970",
                    ValueRegexFilter = @"^2[0-9]{3}-(0[1-9]|1[0-2])$",
                    FormatExample = "1/1/1970[format : dd/mm/yy]",
                    CanEdit = false
                }
            };


            miscs.ForEach(c => context.Miscs.Add(c));

            #endregion
        }

        private static void PaymentTypesSetup(HammondsIBMSContext context)
        {
            #region PaymentTypes

            var pmtTypes = new List<PaymentItemType>();
            for (int i = 1; i <= Enum.GetNames(typeof (PaymentItemTypes)).Length; i++)
            {
                pmtTypes.Add(new PaymentItemType
                {
                    PaymentItemTypeId = i,
                    Description = Enum.GetName(typeof (PaymentItemTypes), i)
                });
            }

            pmtTypes.ForEach(c => context.PaymentItemTypes.Add(c));

            #endregion
        }

        private static void OneOfftypesSetup(HammondsIBMSContext context)
        {
            #region OneOffTypes

            var oneOffTypes = new List<OneOffType>();
            for (int i = 1; i <= Enum.GetNames(typeof (OneOffTypes)).Length; i++)
            {
                oneOffTypes.Add(new OneOffType {OneOffTypeId = i, Description = Enum.GetName(typeof (OneOffTypes), i)});
            }

            oneOffTypes.ForEach(c => context.OneOffTypes.Add(c));

            #endregion
        }

        private static void AccountTypesSetup(HammondsIBMSContext context)
        {
            #region AccountTypes

            var accTypes = new List<TransactionType>();
            for (int i = 1; i <= Enum.GetNames(typeof (TransactionTypes)).Length; i++)
            {
                accTypes.Add(new TransactionType
                {
                    TransactionTypeId = i,
                    Description = Enum.GetName(typeof (TransactionTypes), i)
                });
            }

            accTypes.ForEach(c => context.AccountEntryTypes.Add(c));

            #endregion
        }

        private static void CustomersSetup(HammondsIBMSContext context,List<PaymentSource> paymentSources )
        {
            #region Customers

            var customers = new List<Customer>();
            using (
                var csv =
                    new CsvReader(new StreamReader(HttpContext.Current.Server.MapPath(MigrationFilePath + "/customer.csv")),
                        false))
            {
                while (csv.ReadNextRecord())
                {
                    var firstName = "";
                    if (csv[2].Split(' ').Length > 1)
                        for (int i = 1; i < csv[2].Split(' ').Length; i++)
                        {
                            firstName += csv[2].Split(' ')[i];
                        }

                    customers.Add(new Customer
                    {
                        CustomerId = int.Parse(csv[0]),
                        Surname = csv[2].Split(' ')[0],
                        FirstName = firstName,
                        Address = new Address {AddressLine1 = csv[3]},
                        PreferredPaymentSource = paymentSources[0]
                    });
                }
            }

            customers.ForEach(s => context.Customers.Add(s));

            #endregion
        }

        private static List<PaymentSource> PaymentSourcesSetup(HammondsIBMSContext context)
        {


            var paymentSources = new List<PaymentSource>()
            {
                new PaymentSource {Description = "Cash"},
                new PaymentSource {Description = "Credit Card"},
                new PaymentSource {Description = "Standing Order"},
                new PaymentSource {Description = "Cheque"}
            };
            paymentSources.ForEach(c => context.PaymentSources.Add(c));

            return paymentSources;
        }

        private static void ItemChargesSetup(HammondsIBMSContext context)
        {
            var itemCharges = new List<ItemCharge>
            {
                new ItemCharge {ItemChargeId = 1, Description = "Installation", Amount = 50, PurchaseDefault = true},
                new ItemCharge {ItemChargeId = 2, Description = "Tuning", Amount = 20},
                new ItemCharge {ItemChargeId = 3, Description = "Misc.", Amount = 0}
            };

            itemCharges.ForEach(s => context.ItemCharges.Add(s));
        }

        private static void ContractTypesSetup(HammondsIBMSContext context)
        {
            #region Contract Types

            var contractTypes = new List<ContractType>
            {
                new ContractType
                {
                    ContractTypeId = 1,
                    Description = "Manufacturer Guarantee",
                    PeriodInMonths = 24,
                    NormalTermAmount = 0,
                    PurchaseDefault = true,
                    PaymentPeriodId = 4
                },
                new ContractType
                {
                    ContractTypeId = 2,
                    Description = "Hammonds Free Technical Service",
                    PeriodInMonths = 12,
                    NormalTermAmount = 0,
                    PurchaseDefault = true,
                    PaymentPeriodId = 4
                },
                new ContractType
                {
                    ContractTypeId = 3,
                    Description = "Hammonds Technical Service",
                    PeriodInMonths = 12,
                    NormalTermAmount = 80,
                    PaymentPeriodId = 4
                },
                new ContractType
                {
                    ContractTypeId = 4,
                    Description = "Hammonds Hardware Maintenance",
                    PeriodInMonths = 12,
                    NormalTermAmount = 50,
                    PaymentPeriodId = 4
                },
            };
            contractTypes.ForEach(s => context.ContractTypes.Add(s));

            #endregion
        }

        private static void PaymentPeriodsSetup(HammondsIBMSContext context)
        {
            #region Payment Periods

            var paymentPeriods = new List<PaymentPeriod>
            {
                new PaymentPeriod
                {PaymentPeriodId = 1, Description = "Monthly", PeriodInMonths = 1},
                new PaymentPeriod
                {PaymentPeriodId = 4, Description = "Yearly", PeriodInMonths = 12},
            };
            paymentPeriods.ForEach(s => context.PaymentPeriods.Add(s));

            #endregion
        }

        private static void StockSetup(HammondsIBMSContext context)
        {
            var stocks = new List<Stock>();
            using (
                var csv = new CsvReader(new StreamReader(HttpContext.Current.Server.MapPath(MigrationFilePath + "/stock.csv")),
                    true))
            {
                while (csv.ReadNextRecord())
                {
                    stocks.Add(new Stock
                    {
                        ModelId = int.Parse(csv[1]),
                        SerialNumber = csv[2],
                        Identifier = csv[3],
                        LandedCost = decimal.Parse(csv[4]),
                        ProductLifeCycleId = int.Parse(csv[5]),
                        InvoiceStatusId = int.Parse(csv[6])
                    });
                }
            }
            //var stocks = new List<Stock>
            //                 {
            //                     new Stock
            //                         {
            //                             //StockId = 1,
            //                             Model=models[3],
            //                             SerialNumber = "123456789012",
            //                             Identifier = "456-"+ models[3].ModelCode,
            //                             ProductLifeCycleId = 1,
            //                             InvoiceStatusId = 1,

            //                             //History = new List<History>{new History
            //                             //{
            //                             //       HistoryId = 1,
            //                             //       ProductLifeCycle = pCycles[0],
            //                             //       TimeStamp = DateTime.Now}
            //                             //}
            //                         },
            //                     new Stock
            //                         {
            //                            // StockId = 2,
            //                             Model=models[2],
            //                             SerialNumber = "166656789012",
            //                             Identifier = "555-"+ models[3].ModelCode,
            //                             ProductLifeCycleId = 1,
            //                             InvoiceStatusId=1,

            //                             // History = new List<History>{new History
            //                             //{
            //                             //       HistoryId = 1,
            //                             //       ProductLifeCycle = pCycles[0],
            //                             //       TimeStamp = DateTime.Now}
            //                             //}
            //                         },
            //                     new Stock
            //                         {
            //                            // StockId = 3,
            //                             Model=models[3],
            //                             SerialNumber = "12345679000",
            //                             Identifier = "499-"+ models[3].ModelCode,
            //                             ProductLifeCycleId = 1,
            //                             InvoiceStatusId=1,

            //                             // History = new List<History>{new History
            //                             //{
            //                             //       HistoryId = 1,
            //                             //       ProductLifeCycle = pCycles[0],
            //                             //       TimeStamp = DateTime.Now}
            //                             //}
            //                         },
            //                     new Stock
            //                         {
            //                             //StockId = 4,
            //                             Model=models[3],
            //                             SerialNumber = "736520973649",
            //                             Identifier = "666-"+ models[3].ModelCode,
            //                             ProductLifeCycleId = 1,
            //                             InvoiceStatusId=1,

            //                             // History = new List<History>{new History
            //                             //{
            //                             //       HistoryId = 1,
            //                             //       ProductLifeCycle = pCycles[0],
            //                             //       TimeStamp = DateTime.Now}
            //                             //}
            //                         },
            //                         new Stock
            //                         {
            //                             //StockId = 5,
            //                             Model=models[4],
            //                             SerialNumber = "720973653649",
            //                             Identifier = "667-"+ models[4].ModelCode,
            //                             ProductLifeCycleId = 1,
            //                             InvoiceStatusId=1,

            //                             //History = new List<History>{new History
            //                             //{
            //                             //       HistoryId = 1,
            //                             //       ProductLifeCycle = pCycles[0],
            //                             //       TimeStamp = DateTime.Now}
            //                             //}
            //                         },
            //                         new Stock
            //                         {
            //                            // StockId = 6,
            //                             Model=models[4],
            //                             SerialNumber = "736497365209",
            //                             Identifier = "668-"+ models[4].ModelCode,
            //                             ProductLifeCycleId = 1,
            //                             InvoiceStatusId=1,

            //                             //History = new List<History>
            //                             //{
            //                             //    new History
            //                             //    {
            //                             //       HistoryId = 1,
            //                             //       ProductLifeCycle = pCycles[0],
            //                             //       TimeStamp = DateTime.Now
            //                             //    },
            //                             //    new History
            //                             //        {
            //                             //            HistoryId = 1,
            //                             //           ProductLifeCycle = pCycles[1],
            //                             //           TimeStamp = DateTime.Now
            //                             //        }
            //                             //}
            //                         },


            //                 };

            stocks.ForEach(s => context.Stocks.Add(s));
        }

        private static void ModelsSetup(HammondsIBMSContext context, List<Manufacturer> manufactures)
        {
            var models = new List<Model>
            {
                new Model
                {
                    //ModelId = 1,
                    ModelCode = "KDL-52HX903",
                    Summary =
                        "52” (132cm), Full HD 1080, 3D LCD TV with Intelligent Dynamic Full LED & Motionflow 400 PRO",
                    Manufacturer = manufactures[0],
                    CategoryId = 1,
                    Size = 52,
                    RetailPrice = 1000,
                    RentalPrice = 100,
                    SpainRetailPrice = 1500
                },
                new Model
                {
                    //ModelId = 2,
                    ModelCode = "KDL-55NX813",
                    Summary =
                        "139cm/55, Full HD 1080, 3D LCD TV with Dynamic Edge LED, Monolithic Design & built-in Wi-Fi® access",
                    Manufacturer = manufactures[0],
                    CategoryId = 1,
                    Size = 55
                },
                new Model
                {
                    //ModelId = 3,
                    ModelCode = "LG 55LW650T",
                    Summary = "55\" Cinema 3D LED TV with Smart TV, freeview HD and 200Hz",
                    Manufacturer = manufactures[1],
                    CategoryId = 1,
                    Size = 55,
                    RetailPrice = 2000,
                    RentalPrice = 200,
                    SpainRetailPrice = 2600
                },
                new Model
                {
                    //ModelId = 4,
                    ModelCode = "LG 47LW650T",
                    Summary = "47\" Cinema 3D LED TV with Smart TV, freeview HD and 200Hz",
                    Manufacturer = manufactures[1],
                    CategoryId = 1,
                    Size = 47
                },
                new Model
                {
                    // ModelId = 5,
                    ModelCode = "46PFL9705H/12",
                    Summary = "46\" Full HD 1080p digital TV",
                    Manufacturer = manufactures[2],
                    CategoryId = 1,
                    Size = 46,
                    //Image = (new IbmsImage{Name="46PFL9705H/12"}).LoadFile(HttpContext.Current.Server.MapPath(@"~/DataScaffolding/PopulationData/32PFL5605H_05.jpg"))
                },
                new Model
                {
                    // ModelId = 6,
                    ModelCode = "40PFL9705H/12",
                    Summary = "40\" Full HD 1080p digital TV",
                    Manufacturer = manufactures[2],
                    CategoryId = 1,
                    Size = 40
                },
                new Model
                {
                    //ModelId = 2,
                    ModelCode = "KDL-22EX553",
                    Summary =
                        "55cm / 22\", HD Ready TV with Edge LED, X-Reality, built-in Wi-Fi® and Sony Internet TV",
                    Manufacturer = manufactures[0],
                    CategoryId = 1,
                    Size = 22
                },
                new Model
                {
                    //ModelId = 2,
                    ModelCode = "KDL-32EX653",
                    Summary =
                        "80cm / 32\", Full HD TV with Edge LED, X-Reality, built-in Wi-Fi® and Sony Internet TV",
                    Manufacturer = manufactures[0],
                    CategoryId = 1,
                    Size = 32
                },
                new Model
                {
                    //ModelId = 2,
                    ModelCode = "KDL-40HX753",
                    Summary =
                        "102cm / 40\", Full HD 3D with Dynamic Edge LED, X-Reality, built-in Wi-Fi and Sony Internet TV",
                    Manufacturer =manufactures[0],
                    CategoryId = 1,
                    Size = 40
                },
                new Model
                {
                    //ModelId = 2,
                    ModelCode = "BDP-S790",
                    Summary =
                        "Full HD and 3D with built-in Wi-Fi®, PC streaming, Skype™, Digital Cinema 4K, HDMI® x 2",
                    Manufacturer = manufactures[0],
                    CategoryId = 2,
                    Size = 0
                },
                new Model
                {
                    //ModelId = 2,
                    ModelCode = "BDP-S490",
                    Summary =
                        "Full HD and 3D with access to the Sony Entertainment Network, PC streaming, 2 x USB and Quickstart",
                    Manufacturer = manufactures[0],
                    CategoryId = 2,
                    Size = 0
                },
            };
            models.ForEach(m => context.Models.Add(m));
        }

        private static void CategoriesSetup(HammondsIBMSContext context)
        {
            #region Categories

            var categories = new List<Category>
            {
                new Category {CategoryId = 1, Description = "Television"},
                new Category {CategoryId = 2, Description = "Blue Ray & DVD Player"},
                new Category {CategoryId = 3, Description = "Home Cinema receiver"},
                new Category {CategoryId = 4, Description = "Speakers"},
                new Category {CategoryId = 5, Description = "PVR"}
            };
            categories.ForEach(c => context.Categories.Add(c));

            #endregion
        }

        private static List<Manufacturer> ManufacturersSetup(HammondsIBMSContext context)
        {
            var manufacturers = new List<Manufacturer>
            {
                new Manufacturer {ManufacturerId = 1, Name = "Sony"},
                new Manufacturer {ManufacturerId = 2, Name = "LG"},
                new Manufacturer {ManufacturerId = 3, Name = "Phillips"},
                new Manufacturer {ManufacturerId = 4, Name = "Samsung"},
            };
            //var manufacturers = (from line in File.ReadAllLines("Manufacturers.csv")
            //                     let rec = line.Split(',')
            //                     select new Manufacturer
            //                                {
            //                                    ManufacturerId = int.Parse(rec[0]),
            //                                    Name = rec[1]
            //                                }).ToList();


            manufacturers.ForEach(m => context.Manufacturers.Add(m));

            return manufacturers;
        }

        private static void InvoiceStatusSetup(HammondsIBMSContext context)
        {
            var invStatus = new List<InvoiceStatus>
            {
                new InvoiceStatus {InvoiceStatusId = 1, StatusDescription = "Pending"},
                new InvoiceStatus {InvoiceStatusId = 2, StatusDescription = "Attached"},
                new InvoiceStatus {InvoiceStatusId = 3, StatusDescription = "Closed"}
            };
            invStatus.ForEach(c => context.InvoiceStatuses.Add(c));
        }

        private static void AccountTypesChangeableProductLifeCyclesSetup(HammondsIBMSContext context,List<ProductLifeCycle> pCycles)
        {
            var atcplc = new List<AccountTypeChangeableLifeCycle>
            {
                 new AccountTypeChangeableLifeCycle
                {
                    AccountType = (int) AccountType.Purchase,
                    ProductLifeCycle =
                        pCycles.Find(m => m.ProductLifeCycleId == (int) ProductLifeCycleStatus.Sold)
                },
                 new AccountTypeChangeableLifeCycle
                {
                    AccountType = (int) AccountType.Purchase,
                    ProductLifeCycle =
                        pCycles.Find(m => m.ProductLifeCycleId == (int) ProductLifeCycleStatus.InStock)
                },
                new AccountTypeChangeableLifeCycle
                {
                    AccountType = (int) AccountType.Purchase,
                    ProductLifeCycle =
                        pCycles.Find(m => m.ProductLifeCycleId == (int) ProductLifeCycleStatus.ReRental)
                },
                new AccountTypeChangeableLifeCycle
                {
                    AccountType = (int) AccountType.Rent,
                    ProductLifeCycle =
                        pCycles.Find(m => m.ProductLifeCycleId == (int) ProductLifeCycleStatus.ReRental)
                },
                new AccountTypeChangeableLifeCycle
                {
                    AccountType = (int) AccountType.Rent,
                    ProductLifeCycle =
                        pCycles.Find(m => m.ProductLifeCycleId == (int) ProductLifeCycleStatus.Scrapped)
                },
            };
            atcplc.ForEach(c => context.AccountTypeChangeableLifeCycles.Add(c));
        }

        private static List<ProductLifeCycle> ProductLifeCyclesSetup(HammondsIBMSContext context)
        {
            var pCycles = new List<ProductLifeCycle>()
            {
                new ProductLifeCycle
                {
                    ProductLifeCycleId = 1,
                    ContractChangeable = false,
                    Description = "InStock",
                    DescriptionAbbreviation = "IS",
                    NotAttachedStatus = true,
                    ProductLifeCycleActions = (ProductLifeCycleActions.CanBeSold | ProductLifeCycleActions.CanBeRented),
                },
                new ProductLifeCycle
                {
                    ProductLifeCycleId = 2,
                    ContractChangeable = false,
                    Description = "Sold",
                    DescriptionAbbreviation = "SD",
                    NotAttachedStatus = false,
                    ProductLifeCycleActions=ProductLifeCycleActions.NoAction,
                },
                new ProductLifeCycle
                {
                    ProductLifeCycleId = 3,
                    ContractChangeable = false,
                    Description = "Rented",
                    DescriptionAbbreviation = "RT",
                    NotAttachedStatus = false,
                    ProductLifeCycleActions=ProductLifeCycleActions.NoAction,
                },
                new ProductLifeCycle
                {
                    ProductLifeCycleId = 4,
                    ContractChangeable = true,
                    Description = "ReRental",
                    DescriptionAbbreviation = "RR",
                    NotAttachedStatus = true,
                    ProductLifeCycleActions=ProductLifeCycleActions.CanBeRented,
                },
                new ProductLifeCycle
                {
                    ProductLifeCycleId = 5,
                    ContractChangeable = false,
                    Description = "Scrapped",
                    DescriptionAbbreviation = "SC",
                    NotAttachedStatus = true,
                    ProductLifeCycleActions=ProductLifeCycleActions.NoAction,
                },
                new ProductLifeCycle
                {
                    ProductLifeCycleId = 6,
                    ContractChangeable = false,
                    Description = "Reserved",
                    DescriptionAbbreviation = "RV",
                    NotAttachedStatus = true,
                    ProductLifeCycleActions=ProductLifeCycleActions.NoAction,
                },
            };

            pCycles.ForEach(c => context.ProductLifeCycles.Add(c));
            return pCycles;
        }

        private static void SuppliersSetup(HammondsIBMSContext context)
        {
            var suppliers = new List<Supplier>
            {
                new Supplier
                {
                    SupplierId = 1,
                    Name = "OverClockers UK",
                    Address =
                        new Address
                        {
                            AddressLine1 = "4 Axis - Millennium Way",
                            AddressLine2 = "High Carr Business Park",
                            AddressLine3 = "",
                            City = "Newcastle-under-Lyme",
                            PostCode = "ST5 7UF",
                            Country = "United Kingdom"
                        },
                    ContactInfo = new ContactInfo {Email = "sales@overclockersuk.com", Tel = "0044 871 200 5053"},
                    ExchangeRateId = 1
                },
                new Supplier
                {
                    SupplierId = 2,
                    Name = "Inelec S.A.",
                    Address =
                        new Address
                        {
                            AddressLine1 = "C/ Bocángel, 38",
                            AddressLine2 = "28028 Madrid",
                            City = "Madrid",
                            Country = "Spain"
                        },
                    ContactInfo = new ContactInfo {Email = "manuel.guede@inelec.net", Tel = "0034 91 726 35 00"},
                    ExchangeRateId = 2
                }
            };

            suppliers.ForEach(s => context.Suppliers.Add(s));
        }

        private static void ExchangeRangeSetup(HammondsIBMSContext context)
        {
            var rates = new List<ExchangeRate>
            {
                new ExchangeRate
                {
                    ExchangeRateId = 1,
                    Abbreviation = "GBP",
                    Symbol = "£",
                    CurrencyName = "Gibraltar Pound",
                    RateToGBP = 1
                },
                new ExchangeRate
                {
                    ExchangeRateId = 2,
                    Abbreviation = "EUR",
                    Symbol = "€",
                    CurrencyName = "Euro",
                    RateToGBP = 1.20m
                },
                new ExchangeRate
                {
                    ExchangeRateId = 3,
                    Abbreviation = "USD",
                    Symbol = "$",
                    CurrencyName = "US Dollar",
                    RateToGBP = 1.61m
                }
            };

            rates.ForEach(r => context.ExchangeRates.Add(r));
        }

        private static string GetDocumentTemplateText(string fileName)
        {
            var filePath = MigrationDocTemplates + "/" + fileName;
            var mappedPath = HttpContext.Current.Server.MapPath(filePath);

            var retStr = "";
            using (var tr = File.OpenText(mappedPath))
            {
                retStr = tr.ReadToEnd();
            }

            return retStr;
        }

    }
}

//#region Contract Units
//stocks[4].ProductLifeCycleId = (int)ProductLifeCycleStatus.Sold;
//stocks[5].ProductLifeCycleId = (int) ProductLifeCycleStatus.Rented;
//var contractUnits = new List<ContractUnit>
//                        {

//                            new ContractUnitPurchase { CustomerAccountId = 1,CustomerId = 1, Stock = stocks[4]},
//                            //new ContractUnitRental {CustomerAccountId = 2,CustomerId = 1,Stock =stocks[5],Rental = new Rental { StartDate = DateTime.Parse("2011-5-1"), MonthlyCharge = 50, PaymentPeriodId = 1 } }

//                        };

//contractUnits.ForEach(s => context.ContractUnits.Add(s));

//#endregion



//#region ServiceContracts

//var contracts = new List<Contract>
//                    {
//                        new Contract
//                            {
//                                //ContractId = 1,
//                                ContractTypeId = 1,
//                                CustomerAccountId = contractUnits[0].CustomerAccountId,
//                                StartDate = DateTime.Parse("2011-1-1"),
//                                ContractLengthInMonths = 24,
//                                PaymentPeriodId = 1

//                            },
//                        new Contract
//                            {
//                                //ContractId = 1,
//                                ContractTypeId = 2,
//                                CustomerAccountId = contractUnits[0].CustomerAccountId,
//                                StartDate = DateTime.Parse("2011-1-1"),
//                                ContractLengthInMonths = 12,
//                                PaymentPeriodId = 2
//                            }
//                    };
//contracts.ForEach(s => context.Contracts.Add(s));

//#endregion