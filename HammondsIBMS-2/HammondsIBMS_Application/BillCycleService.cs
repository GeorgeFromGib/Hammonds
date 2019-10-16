using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using HammondsIBMS_Data.Infrastructure;
using HammondsIBMS_Data.Repositories;
using HammondsIBMS_Domain.Entities.Accounts;
using HammondsIBMS_Domain.Entities.BillCycles;
using HammondsIBMS_Domain.Entities.Contracts;
using HammondsIBMS_Domain.Entities.Customers;
using HammondsIBMS_Domain.Entities.Invoicing;
using HammondsIBMS_Domain.Entities.Misc;
using HammondsIBMS_Domain.Repositories;

namespace HammondsIBMS_Application
{

    public delegate void BillCycleRunHandler(object sender,bool isRunning, int noOfCustomersInvoiced);

    public class BillCycleService : ServiceBase
    {
        private readonly CustomerService _customerService;
        private readonly TransactionsService _transactionsService;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly ICustomerAccountRepository _contractUnitRepository;
        private readonly IPostedRepository _postedRepository;
        private readonly IBillCycleRunRepository _billCycleRunRepository;
        private readonly MiscService _miscService;

        public BillCycleService(IUnitOfWork unitOfWork, 
            CustomerService customerService, 
            TransactionsService transactionsService, 
            IInvoiceRepository invoiceRepository, 
            ICustomerAccountRepository contractUnitRepository,
            IPostedRepository postedRepository,
            IBillCycleRunRepository billCycleRunRepository,MiscService miscService)
            : base(unitOfWork)
        {
            _customerService = customerService;
            _transactionsService = transactionsService;
            _invoiceRepository = invoiceRepository;
            _contractUnitRepository = contractUnitRepository;
            _postedRepository = postedRepository;
            _billCycleRunRepository = billCycleRunRepository;
            _miscService = miscService;
        }

        public event BillCycleRunHandler BillCycleRunChanged;

        public void OnBillCycleRunChanged(bool isRunning, int noofcustomersinvoiced)
        {
            BillCycleRunHandler handler = BillCycleRunChanged;
            if (handler != null) 
                handler(this, isRunning, noofcustomersinvoiced);
        }

        #region BillCycle run

      

        public void ExecuteBillCycleRun(BillCycle billCycle)
        {
            var noOfCustomersInvoiced = 0;
            decimal totalInvoiced = 0.0m;

            OnBillCycleRunChanged(true,0);

            var customers = GetCustomersToInvoice();
            foreach (var customer in customers)
            {

                var invoice = CreateInvoice(billCycle, customer);

                AddCustomerActiveContractsToInvoiceAndUpdateAccounts(invoice, customer);

                var prevBal = _transactionsService.GetBalanceForCustomer(invoice.CustomerId);

                if (invoice.InvoiceItems.Count > 0 || prevBal != 0)
                {
                    invoice.PreviousBalance = prevBal;
                    _invoiceRepository.Add(invoice);
                    totalInvoiced += invoice.InvoicedTotal;
                    noOfCustomersInvoiced++;
                    OnBillCycleRunChanged(true, noOfCustomersInvoiced);
                }
            }

            AddBillCycleRun(billCycle, noOfCustomersInvoiced, totalInvoiced);
            UpdateLastBillCycle(billCycle);

            OnBillCycleRunChanged(false,noOfCustomersInvoiced);
        }

        private void UpdateLastBillCycle(BillCycle billCycle)
        {
            _miscService.UpdateValue(MiscIdentifier.LastBillCycle, billCycle.ToString());
        }

        public int GetNoOfCustomersToInvoice()
        {
            return GetCustomersToInvoice().Count();
        }

        private IEnumerable<Customer> GetCustomersToInvoice()
        {
            return _customerService.GetCustomers();
        }

        private void AddBillCycleRun(BillCycle billCycle, int noOfCustomersInvoiced, decimal totalInvoiced)
        {
            var bcr = new BillCycleRun
                          {
                              BillCycle = billCycle,
                              AmountInvoiced = totalInvoiced,
                              NoOfCustomersInvoiced = noOfCustomersInvoiced,
                              DateOfRun = DateTime.Today
                          };
            _billCycleRunRepository.Add(bcr);
        }

        private static Invoice CreateInvoice(BillCycle billCycle, Customer customer)
        {
            return new Invoice
            {
                CustomerId = customer.CustomerId,
                BillCycle = billCycle,
                InvoiceItems = new List<InvoiceItem>(),
                PostedItems = new List<InvoicePosted>(),
                InvoiceDate = DateTime.Today
            };
        }

        private void AddCustomerActiveContractsToInvoiceAndUpdateAccounts(Invoice invoice, Customer customer)
        {
            var activeContracts = _contractUnitRepository.GetMany(c => c.CustomerId == customer.CustomerId);
            foreach (var activeContract in activeContracts)
            {
                var invoiceItem = InitialiseInvoiceItem(activeContract);

                AddPostedEntriesToContracts(invoice, invoiceItem, activeContract);

                AddContractsToInvoice(invoice, invoiceItem, activeContract);

                if (AreThereContractItemsInInvoiceItem(invoiceItem))
                {
                    invoice.InvoiceItems.Add(invoiceItem);
                    AddTransactionEntryForInvoice(invoice, invoiceItem);
                }
            }
        }

        private void AddTransactionEntryForInvoice(Invoice invoice, CustomerAccountInvoiceItem invoiceItem)
        {
            _transactionsService.Add(new Transaction
            {
                Amount = invoiceItem.Amount,
                CustomerAccountId = invoiceItem.CustomerAccountId,
                TransactionTypeId = (int)TransactionTypes.Invoice,
                DateTime = invoice.InvoiceDate,
                BillCycle = invoice.BillCycle,
                Description = string.Format("Bill cycle : {0} invoice", invoice.BillCycle.ToString())
            });
        }

        private static bool AreThereContractItemsInInvoiceItem(CustomerAccountInvoiceItem invoiceItem)
        {
            return invoiceItem.AttachedContractItems.Count > 0;
        }

        private static CustomerAccountInvoiceItem InitialiseInvoiceItem(CustomerAccount activeContract)
        {
            return new CustomerAccountInvoiceItem
            {
                CustomerAccountId = activeContract.CustomerAccountId,
                CustomerAccountDetails = activeContract.ContractUnitDescription + " - " + activeContract.ModelDescription,
                AttachedContractItems = new List<InvoiceAttachedContractItems>()
            };
        }

        private static void AddContractsToInvoice(Invoice invoice, CustomerAccountInvoiceItem invoiceItem,
                                                  CustomerAccount activeContract)
        {
            if (activeContract.WasActiveOn(invoice.BillCycle.Date))
            {
                var contractsForBilling = activeContract.GetListOfContractsToBill(invoice.BillCycle).ToList();
                //TODO Review. invoiceItem.AttachedContractItems.AddRange(
                    //contractsForBilling.Select(c => c.PopulateInvoiceAttachedContractItems()).ToList());
                contractsForBilling.ForEach(c => c.LastBilled = invoice.BillCycle);
            }
        }

        private void AddPostedEntriesToContracts(Invoice invoice, CustomerAccountInvoiceItem invoiceItem,
                                                          CustomerAccount activeContract)
        {
            throw new NotImplementedException();
            //var posted = _postedRepository.GetMany(p => p.Invoice == null && p.Contract.CustomerAccountId == activeContract.CustomerAccountId);
            //foreach (var adjustmentPost in posted)
            //{
            //    if (adjustmentPost.Contract.IsBillableThisCycle(invoice.BillCycle) || (adjustmentPost.Contract.IsContractExpiredOnDate(invoice.BillCycle.Date)))
            //    {
            //        invoiceItem.AttachedContractItems.Add(new InvoiceAttachedContractItems
            //        {
            //            Description = adjustmentPost.InvoiceString,
            //            Amount = adjustmentPost.GetInvoicedAmount(invoice.BillCycle)
            //        });
            //        adjustmentPost.Invoice = invoice;
            //        _postedRepository.Update(adjustmentPost);
            //    }
            //}
        }

        #endregion

        public IEnumerable<BillCycleRun> GetBillCycleHistory()
        {
            return _billCycleRunRepository.GetAll();
        }



        public BillCycle GetLastBillCycle()
        {
            return _miscService.GetLastBillCycle();
        }

        public int GetMinimumNoOfDaysBetweenBillingCycles()
        {
            return _miscService.GetMinNoDaysBetweenBillsCycles();
        }

    }
}