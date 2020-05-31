using System;
using System.Collections.Generic;
using System.Linq;
using HammondsIBMS_Data.Infrastructure;
using HammondsIBMS_Data.Repositories;
using HammondsIBMS_Domain.Entities.Stock;
using HammondsIBMS_Domain.Model.Stock;
using HammondsIBMS_Domain.Model.Supplier;
using HammondsIBMS_Domain.Repositories;

namespace HammondsIBMS_Application
{
    public interface IStockInvoiceService
    {
        IEnumerable<StockInvoice> GetAllInvoices();

        void CommitChanges();

        void AddInvoice(StockInvoice entity);

        IEnumerable<Stock> GetPendingStockItems();

        Stock GetStock(int id);

        StockInvoice GetInvoice(int p);

        void UpdateInvoice(StockInvoice entity);

        IEnumerable<Stock> GetSelectableStock();

        StockInvoiceItem GetInvoiceItem(int id);

        void DeleteInvoice(StockInvoice entity);

        void CloseInvoice(StockInvoice entity);
        void CancelInvoice(StockInvoice entity);
    }

    public class StockInvoiceService:ServiceBase, IStockInvoiceService
    {
        private readonly IStockInvoiceRepository _stockInvoiceRepository;
        private readonly StockService _stockService;

        public StockInvoiceService(
            IStockInvoiceRepository stockInvoiceRepository
            ,StockService stockService
            ,IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _stockInvoiceRepository = stockInvoiceRepository;
            _stockService = stockService;
        }

        public IEnumerable<StockInvoice> GetAllInvoices()
        {
            return _stockInvoiceRepository.GetAll();
        }


        public void AddInvoice(StockInvoice entity)
        {
            _stockInvoiceRepository.Add(entity);
        }

        public IEnumerable<Stock> GetPendingStockItems()
        {
            const int inStock = (int)ProductLifeCycleStatus.InStock;
            var k = from s in _stockService.GetStocks() where s.ProductLifeCycleStatus.ProductLifeCycleId == inStock select s;
            return k;
        }

        public Stock GetStock(int id)
        {
            return _stockService.GetStock(id);
        }

        public StockInvoice GetInvoice(int id)
        {
            return _stockInvoiceRepository.GetById(id);
        }

       

        public IEnumerable<Stock> GetSelectableStock()
        {
            const int pending = (int) InvoiceStatusEnum.Pending;
            var sel = _stockService.GetStocks().Where(s => s.InvoiceStatusId == pending).ToList();
            //var sel = from s in _stockService.GetStocks() where s.InvoiceStatusId == pending select s;
            return sel;
        }

        public StockInvoiceItem GetInvoiceItem(int id)
        {
            return _stockInvoiceRepository.GetInvoiceItem(id);
        }

        public void DeleteInvoice(StockInvoice entity)
        {
            _stockInvoiceRepository.Delete(entity);
        }

        public void UpdateInvoice(StockInvoice entity)
        {
            foreach (var invItm in entity.Items)
            {
                UpdateStockModel(invItm);
            }
            _stockInvoiceRepository.Update(entity);
        }

        public void CloseInvoice(StockInvoice entity)
        {
            entity.IsProcessed = true;
            CloseInvoiceAndUpdateInventory(entity);
        }

        public void CancelInvoice(StockInvoice entity)
        {
            entity.IsCancelled = true;
            CloseInvoiceAndUpdateInventory(entity);
        }


        private void CloseInvoiceAndUpdateInventory(StockInvoice entity)
        {

            foreach (var invItm in entity.Items)
            {
                foreach (var stkItms in invItm.StockItems)
                {
                    if(entity.IsCancelled)
                    {
                        stkItms.InvoiceStatusId = (int) InvoiceStatusEnum.Pending;
                    }
                    else
                    {
                        stkItms.LandedCost = invItm.LandedCost;
                        stkItms.InvoiceStatusId = (int)InvoiceStatusEnum.Closed;
                    }
                }
            }
            entity.DateProcessed = DateTime.Now;
            UpdateInvoice(entity);
        }

        private void UpdateStockModel(StockInvoiceItem invItm)
        {
            var model = _stockService.GetModel(invItm.StockModelId);
            model.RentalPrice = invItm.RentalPrice;
            model.RetailPrice = invItm.RetailPrice;
            model.SpainRetailPrice = invItm.SpainRetailPrice;
            _stockService.UpdateModel(model);
        }
    }
}
