using System.Collections.Generic;
using HammondsIBMS_Data.Infrastructure;
using HammondsIBMS_Data.Repositories;
using HammondsIBMS_Domain.Entities.Invoicing;
using HammondsIBMS_Domain.Entities.Misc;
using HammondsIBMS_Domain.Repositories;

namespace HammondsIBMS_Application
{
    public class MiscService : ServiceBase
    {
        private readonly IMiscRepository _miscRepository;

        public MiscService(IUnitOfWork unitOfWork,IMiscRepository miscRepository) : base(unitOfWork)
        {
            _miscRepository = miscRepository;
        }

        public decimal GetDefaultDutyPercentage()
        {
            return decimal.Parse(GetValue(MiscIdentifier.DefaultDutyPercentage));
        }

        public BillCycle GetLastBillCycle()
        {
            var lbc = GetValue(MiscIdentifier.LastBillCycle);
            return new BillCycle(lbc);
        }

        public int GetMinNoDaysBetweenBillsCycles()
        {
            return int.Parse(GetValue(MiscIdentifier.MinPeriodForBillCycles));
        }

        public int GetContractExpiryPeriod()
        {
            return int.Parse(GetValue(MiscIdentifier.ContractExpiryWarningPeriod));
        }
        
        public decimal GetDefaultDepositForRentalUnits()
        {
            return decimal.Parse(GetValue(MiscIdentifier.DefaultDepositPerUnit));
        }

        public void UpdateValue(MiscIdentifier miscIdentifier,string value)
        {
            var msc = GetById((int) miscIdentifier);
            msc.Value = value;
            Update(msc);
        }

        public void Update(Misc misc)
        {
            _miscRepository.Update(misc);
        }

        public IEnumerable<Misc> GetAll()
        {
            return _miscRepository.GetAll();
        }

        public Misc GetById(int id)
        {
            return _miscRepository.GetById(id);
        }

        private string GetValue(MiscIdentifier miscIdentifier)
        {
            return _miscRepository.GetById((int) miscIdentifier).Value;
        }


        
    }
}