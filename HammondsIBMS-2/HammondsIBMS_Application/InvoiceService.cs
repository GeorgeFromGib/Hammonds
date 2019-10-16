using System;
using System.Collections.Generic;
using System.Linq;
using HammondsIBMS_Data.Infrastructure;
using HammondsIBMS_Data.Repositories;
using HammondsIBMS_Domain.Entities.Contracts;
using HammondsIBMS_Domain.Entities.Invoicing;
using HammondsIBMS_Domain.Repositories;

namespace HammondsIBMS_Application
{
    public class InvoiceService:ServiceBase
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IPostedRepository _postedRepository;


        public InvoiceService(IUnitOfWork unitOfWork,IInvoiceRepository invoiceRepository,IPostedRepository postedRepository) : base(unitOfWork)
        {
            _invoiceRepository = invoiceRepository;
            _postedRepository = postedRepository;
        }

        
       
        public IEnumerable<Invoice> GetAll()
        {
            return _invoiceRepository.GetAll();
        }

        public IEnumerable<Invoice> GetInvoicesForCustomer(int id)
        {
            return _invoiceRepository.GetMany(i => i.CustomerId == id);

        }

        public Invoice GetInvoice(int id)
        {
            return _invoiceRepository.GetById(id);
        }

        public InvoiceItem GetInvoiceItem(int id)
        {
            return _invoiceRepository.GetInvoiceItemById(id);
        }

        public IEnumerable<AdjustmentPost> GetAllPosted()
        {
            return _postedRepository.GetAll();
        }

        public IEnumerable<AdjustmentPost> GetPostedByCustomer(int customerId)
        {
            throw new NotImplementedException();
            //return _postedRepository.GetMany(p => p.Contract.CustomerAccount.CustomerId == customerId);
        }

        public AdjustmentPost GetPostedById(int id)
        {
            return _postedRepository.GetById(id);
        }


        public void DeletePosted(AdjustmentPost post)
        {
            _postedRepository.Delete(post);
        }
    }
}