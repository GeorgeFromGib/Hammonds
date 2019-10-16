using System.Collections.Generic;
using HammondsIBMS_Domain.Entities.Contracts;
using HammondsIBMS_Domain.Model.Stock;

namespace HammondsIBMS_Domain.Entities.Stock
{
    public class AccountTypeChangeableLifeCycle
    {
        public int AccountTypeChangeableLifeCycleId { get; set; }
        public int AccountType { get; set; }
        public virtual ProductLifeCycle ProductLifeCycle { get; set; }
         
    }
}