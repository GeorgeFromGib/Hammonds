using System.Collections.Generic;
using HammondsIBMS_Data.Repositories;
using HammondsIBMS_Domain.Entities.Contracts;
using HammondsIBMS_Domain.Repositories;

namespace HammondsIBMS_Application
{
    public class ItemChargeService : ServiceBase
    {
        private readonly IItemChargeRepository _itemChargeRepository;

        public ItemChargeService(IUnitOfWork unitOfWork,IItemChargeRepository itemChargeRepository) : base(unitOfWork)
        {
            _itemChargeRepository = itemChargeRepository;
        }

        public IEnumerable<ItemCharge> GetItemCharges()
        {
            return _itemChargeRepository.GetAll();
        }

        public void AddItemCharge(ItemCharge itemCharge)
        {
            _itemChargeRepository.Add(itemCharge);
        }

        public void UpdateItemCharge(ItemCharge itemCharge)
        {
            _itemChargeRepository.Update(itemCharge);
        }

        public ItemCharge GetItemCharge(int id)
        {
            return _itemChargeRepository.GetById(id);
        }
    }
}