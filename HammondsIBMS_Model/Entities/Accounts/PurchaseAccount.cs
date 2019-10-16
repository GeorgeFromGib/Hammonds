using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using FluentValidation;
using FluentValidation.Attributes;
using HammondsIBMS_Domain.Entities.Contracts;
using HammondsIBMS_Domain.Model.Stock;

namespace HammondsIBMS_Domain.Entities.Accounts
{
    [Table("PurchaseAccounts")]
    [Validator(typeof(PurchaseAccountValidator))]
    public class PurchaseAccount:CustomerAccount
    {
        
        public virtual List<PurchaseUnit> PurchasedUnits { get; set; }

      

        public override string ModelDescription
        {
            get
            {
                if (PurchasedUnits != null)
                {
                    if (PurchasedUnits.Count(c => c.RemovalDate == null) > 1)
                        return "[Multi Units]";
                    else
                        return PurchasedUnits.Count(c => c.RemovalDate == null) > 0 ? PurchasedUnits.Single(c => c.RemovalDate == null).Stock.ManufacturerModel : "";
                }
                return "";
            }
        }

        public override AccountType AccountType
        {
            get { return AccountType.Purchase;}
        }


        public override void SetStockAppropriateLifeCycle()
        {
            PurchasedUnits.ForEach(u=>u.Stock.ProductLifeCycleId= (int)ProductLifeCycleStatus.Sold);
        }

        public override bool IsActive
        {
            get
            {
                if(AttachedContracts!=null)
                    if(AttachedContracts.Any(c=>c.ContractStatus.ContractStatusId==(int)ContractStatus.ContractStatuses.Active))
                        return true;
                return false;
            }
        }

        public override bool WasActiveOn(DateTime dateTime)
        {
            if (AttachedContracts.Any(c => c.GetContractStatusOn(dateTime).ContractStatusId == (int)ContractStatus.ContractStatuses.Active))
                return true;
            return false;
        }

        public PurchaseAccount()
        {
            PurchasedUnits=new List<PurchaseUnit>();
        }
    }

    public class PurchaseAccountValidator : AbstractValidator<PurchaseAccount>
    {
        public PurchaseAccountValidator()
        {
            //RuleFor(x => x.StockId).GreaterThan(0).WithMessage("Stock Unit has not been selected!");
            //RuleFor(x => x.Stock.CustomerAccountId).Must(m => m == null).WithMessage("Stock already allotted to another account");
        }
    }
}