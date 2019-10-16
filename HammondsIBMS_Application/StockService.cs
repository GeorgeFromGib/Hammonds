using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using HammondsIBMS_Data.Infrastructure;
using HammondsIBMS_Data.Repositories;
using HammondsIBMS_Domain.BaseInterfaces;
using HammondsIBMS_Domain.Entities.Contracts;
using HammondsIBMS_Domain.Entities.Documents;
using HammondsIBMS_Domain.Entities.Stock;
using HammondsIBMS_Domain.Model.Product;
using HammondsIBMS_Domain.Model.Stock;
using HammondsIBMS_Domain.Repositories;

namespace HammondsIBMS_Application
{
   

    public class StockService : ServiceBase
    {
        private readonly IManufacturerRepository _manufacturerRepository;
        private readonly IModelRepository _modelRepository;
        private readonly IStockRepository _stockRepository;
        private readonly IDocumentService _imageService;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IModelSpecificContractRepository _modelSpecificContractRepository;

        public StockService(IManufacturerRepository manufacturerRepository
            ,IModelRepository modelRepository  
            ,IStockRepository stockRepository
            ,IDocumentService imageService
            ,ICategoryRepository categoryRepository
            ,IModelSpecificContractRepository modelSpecificContractRepository
            ,IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _manufacturerRepository = manufacturerRepository;
            _modelRepository = modelRepository;
            _stockRepository = stockRepository;
            _imageService = imageService;
            _categoryRepository = categoryRepository;
            _modelSpecificContractRepository = modelSpecificContractRepository;
        }

        #region Manufacturer

        public IEnumerable<Manufacturer> GetManufacturers()
        {
            return _manufacturerRepository.GetAll().Where(m=>!m.Retired);
        }

        public IEnumerable<Manufacturer> GetAllManufacturers()
        {
            return _manufacturerRepository.GetAll();
        }

        public void UpdateManufacturer(Manufacturer item)
        {
            _manufacturerRepository.Update(item);
        
        }

        public void AddManufacturer(Manufacturer item)
        {
            _manufacturerRepository.Add(item);
        }

        public Manufacturer GetManufacturer(int id)
        {
            return _manufacturerRepository.GetById(id);
        }


        public void DeleteManufacturer(Manufacturer mdl)
        {
            _manufacturerRepository.Delete(mdl);
        }

        #endregion

        #region Model

        public IEnumerable<Model> GetModels()
        {
            return  _modelRepository.GetAll();
        }

        public Model GetModel(int id)
        {
            return _modelRepository.GetById(id);
        }

        public void AddModel(Model item)
        {
            ValidateModelEntry(item);
            _modelRepository.Add(item);
        }

        public void UpdateModel(Model item)
        {
            ValidateModelEntry(item);
            _modelRepository.Update(item);
        }

        private void ValidateModelEntry(Model item)
        {
            if (_modelRepository.GetMany(m => m.ModelId != item.ModelId && m.ModelCode == item.ModelCode).Any())
                throw new BusinessRuleException("ModelCode", "Duplicate Model Code entered");
        }

        public void DeleteModel(Model item)
        {
            if(_stockRepository.GetMany(m=>m.ModelId==item.ModelId).Any())
                throw new BusinessRuleException("","Model has a stock history and can no be removed!");
            _modelRepository.Delete(item);
        }

        public void DeleteImage(int ibmsImageId)
        {
            _imageService.DeleteDocument(ibmsImageId);
        }

        #endregion

        #region Model Specific Contracts

        public ModelSpecificContract GetModelSpecificContract(int contractSpecificId)
        {
            return _modelSpecificContractRepository.GetById(contractSpecificId);
        }

        public void UpdateModelSpecificContract(ModelSpecificContract modelSpecificContract)
        {
            _modelSpecificContractRepository.Update(modelSpecificContract);
        }

        public void DeleteModelSpecificContract(int modelSpecificContractId)
        {
            _modelSpecificContractRepository.Delete(m=>m.ModelSpecificContractId==modelSpecificContractId);
        }


        #endregion

        #region Stock

        public IEnumerable<Stock> GetStocks()
        {
            return  _stockRepository.GetAll();
        }

        public Stock GetStock(int id)
        {
            return _stockRepository.GetById(id);
        }

        public IEnumerable<Stock> FindStock(Expression<Func<Stock,bool>> where)
        {
            return _stockRepository.GetMany(where);
        }

        public void AddStock(Stock entity)
        {
            ValidateStockEntity(entity);
            UpdateStockHistory(entity);
            _stockRepository.Add(entity);
        }

        private void ValidateStockEntity(Stock entity)
        {
            if (_stockRepository.GetMany(m => m.StockId != entity.StockId && m.Identifier == entity.Identifier).Any())
                throw new BusinessRuleException("Identifier", "Identifier must be unique");
            if (_stockRepository.GetMany(m => m.StockId != entity.StockId && m.SerialNumber == entity.SerialNumber).Any())
                throw new BusinessRuleException("SerialNumber","Serial Number must be unique");
        }


        public void UpdateStock(Stock entity)
        {
            ValidateStockEntity(entity);
            UpdateStockHistory(entity);
            _stockRepository.Update(entity);
        }

        private static void UpdateStockHistory(Stock entity)
        {
            var entHist = entity.History ?? new List<StockHistory>();
            entHist.Add(new StockHistory {ProductLifeCycleId = entity.ProductLifeCycleId, TimeStamp = DateTime.Today});
        }

        public void ChangeStockLifeCycle(int stockId,int stockProductCycleId)
        {
            var stk = GetStock(stockId);
            stk.ProductLifeCycleId = stockProductCycleId;
            UpdateStock(stk);
        }

        public ModelImage GetModelImage(int id)
        {
            return _imageService.GetDocument(id) as ModelImage;
        }

        #endregion

        public IEnumerable<ProductLifeCycle> GetProductLifeCycles()
        {
            return _stockRepository.AllProductLifeCycles();
        }

        public void SetCustomerAccountId(int? customerAccountId,int stockId)
        {
            var stk = _stockRepository.GetById(stockId);
            stk.CustomerAccountId = customerAccountId;
            UpdateStock(stk);
        }

       

        #region Category
        public IEnumerable<Category> GetCategories()
        {
            return _categoryRepository.GetAll();
        }

        public Category GetCategory(int id)
        {
            return _categoryRepository.GetById(id);
        }

        public void AddCategory(Category category)
        {
            _categoryRepository.Add(category);
        }

        public void UpdateCategory(Category category)
        {
            _categoryRepository.Update(category);
        }

        public void DeleteCategory(Category category)
        {
            _categoryRepository.Delete(category);
        }

#endregion





    }
}
