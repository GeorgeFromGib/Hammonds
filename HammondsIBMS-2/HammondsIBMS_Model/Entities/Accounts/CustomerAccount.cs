using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using HammondsIBMS_Domain.BaseInterfaces;
using HammondsIBMS_Domain.Entities.AccountTransactions;
using HammondsIBMS_Domain.Entities.Contracts;
using HammondsIBMS_Domain.Entities.Customers;
using HammondsIBMS_Domain.Entities.Invoicing;
using HammondsIBMS_Domain.Values;

namespace HammondsIBMS_Domain.Entities.Accounts
{
    public abstract class CustomerAccount 
    {
  
        public string ContractUnitDescription
        {
            get { return "Account : " + CustomerAccountId + "- " + AccountType; }
        }

        public abstract string ModelDescription { get; }


        public int CustomerAccountId { get; set; }

        public abstract AccountType AccountType { get; }

        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        public virtual List<Contract> AttachedContracts { get; set; }

        public virtual List<OneOffItem> OneOffItems { get; set; }


        public DateTime OpenedDate { get; set; }

        
        [UIHint("_Address")]
        public Address AlternateAddress { get; set; }

        [DisplayName("Instructions")]
        [DataType(DataType.MultilineText)] 
        public string AlternateAddressInstructions { get; set; }

        public string Notes { get; set; }

        [DisplayName("Account Active?")]
        public abstract bool IsActive { get; }

        [ScaffoldColumn(false)]
        public bool IsTemp { get; set; }


        public CustomerAccount()
        {
            AttachedContracts=new List<Contract>();
            AlternateAddress=new Address{AddressLine1 = ""};
            IsTemp = false;
        }

        public virtual bool HasAlternateAddress()
        {
            return !string.IsNullOrEmpty(AlternateAddress.AddressLine1);
        }

        public virtual bool HasInstructions()
        {
            return !string.IsNullOrEmpty(AlternateAddressInstructions);
        }

        public virtual List<Transaction> Transactions { get; set; } 

        public virtual List<AccountTransaction> AccountTransactions { get; set; }

        public abstract bool WasActiveOn(DateTime dateTime);

        public virtual IEnumerable<Contract> GetListOfContractsToBill(BillCycle billCycle)
        {
            return AttachedContracts.Where(attachedContract => attachedContract.IsBillableThisCycle(billCycle));
        }


        public virtual void AddContract(Contract contract)
        {
            if(contract==null) return;
            contract.LastBilled = contract.LastBilled ?? new BillCycle(ConstantValues.MinDate);
            AttachedContracts = AttachedContracts ?? new List<Contract>();
            AttachedContracts.Add(contract);
        }


        public abstract void SetStockAppropriateLifeCycle();

        public virtual decimal TotalOneOffItems
        {
            get
            {
                if(OneOffItems==null) return 0;
                return OneOffItems.Sum(o => o.TotalCharge);
            }
        }

    }
}