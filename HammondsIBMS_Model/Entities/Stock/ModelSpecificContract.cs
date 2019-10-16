using HammondsIBMS_Domain.Entities.Contracts;

namespace HammondsIBMS_Domain.Entities.Stock
{
    public partial class ModelSpecificContract
    {
        public int ModelSpecificContractId { get; set; }
        public int ContractTypeId { get; set; }
        public virtual ContractType ContractType { get; set; }
        public bool Default { get; set; }
        public int PeriodInMonths { get; set; }
        public decimal NormalTermAmount { get; set; }
    }
}