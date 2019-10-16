using System;
using System.Collections.Generic;

namespace HammondsIBMS_Domain.Entities.Contracts
{
    public class ContractStatus
    {
        private static Dictionary<int,ContractStatus> _statuses=new Dictionary<int, ContractStatus>();
        private readonly ContractStatuses _contractStatuses;

         public enum ContractStatuses
         {
               Active = 1,
               Expired = 2,
               Scheduled = 3
         }

        public int ContractStatusId { get; set; }
        
        
        public string Description
        {
            get { return Enum.GetName(typeof (ContractStatuses), _contractStatuses).ToString(); }
        }

        private ContractStatus(ContractStatuses contractStatuses)
        {
            ContractStatusId = (int) contractStatuses;
            _contractStatuses = contractStatuses;
        }

        public static ContractStatus CreateContractStatus(ContractStatuses contractStatuses)
        {
            ContractStatus con;
            var statusVal = (int)contractStatuses;
            if(!_statuses.TryGetValue(statusVal,out con))
            {
                con = new ContractStatus(contractStatuses);
                _statuses.Add(statusVal,con);
            }
            return con;
        }
    }
   
}