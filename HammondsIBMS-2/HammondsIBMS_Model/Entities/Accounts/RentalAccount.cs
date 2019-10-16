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
using HammondsIBMS_Domain.Values;

namespace HammondsIBMS_Domain.Entities.Accounts
{
    [Table("RentalAccounts")]
    [Validator(typeof(RentalAccountValidator))]
    public class RentalAccount : CustomerAccount
    {

        public override AccountType AccountType
        {
            get { return AccountType.Rent; }
        }

        [DisplayName("Model")]
        public override string ModelDescription
        {
            get
            {
                if (RentedUnits != null)
                {
                    if (RentedUnits.Count(c => c.RemovalDate == null) > 1)
                        return "[Multi Units]";
                    else
                        return RentedUnits.Count(c => c.RemovalDate == null) > 0 ? RentedUnits.Single(c => c.RemovalDate == null).Stock.ManufacturerModel : "";
                }
                return "";
            }
        }


        public override bool IsActive
        {
            get
            {
                return (RentedContract!=null && RentedContract.ExpiryDate > DateTime.Today && RentedContract.StartDate < DateTime.Today);
            }
        }

        public override bool WasActiveOn(DateTime dateTime)
        {
            return RentedContract.GetContractStatusOn(dateTime).ContractStatusId == (int)ContractStatus.ContractStatuses.Active;
        }

        public virtual List<RentedUnit> RentedUnits { get; set; } 


      
        public override void SetStockAppropriateLifeCycle()
        {
            RentedUnits.ForEach(x=>x.Stock.ProductLifeCycleId=(int)ProductLifeCycleStatus.Rented);
        }

        public RentalContract RentedContract
        {
            get { return GetActiveRentalContract() ; }
            set
            {
                var arc = GetActiveRentalContract();
                if (arc == null)
                    AddContract(value);
                else
                {
                    var idx = AttachedContracts.IndexOf(arc);
                    AttachedContracts[idx] = value;
                }

            }
        }

        private RentalContract GetActiveRentalContract()
        {
            if (AttachedContracts != null && AttachedContracts.Count > 0)
                return AttachedContracts.Find(c=>c.ExpiryDate==null) as RentalContract;
            return null;
        }


        [DisplayName("Monthly Charge")]
        [DataType(DataType.Currency)]
        [UIHint("Money")]
        public decimal MonthlyCharge
        {
            get
            {
                return RentedUnits!=null ? RentedUnits.Where(c=>c.RemovalDate==null).Sum(c => c.Amount) : 0;
            }
        }

 

        [DisplayName("Total Rental")]
        [DataType(DataType.Currency)]
        public virtual decimal TotalRental
        {
            get { return RentedUnits == null ? 0 : RentedUnits.Where(c=>c.RemovalDate==null).Sum(c => c.Amount); }
        }

        [DisplayName("Total Deposit")]
        [DataType(DataType.Currency)]
        public decimal TotalDeposit
        {
            get { return RentedUnits == null ? 0 : RentedUnits.Sum(c => c.Deposit); }
        }

        public bool RequiresInvoicingProErrata { get; set; }

        public decimal RatePerDay
        {
            get { return MonthlyCharge/(decimal)ConstantValues.AvgDaysPerMonth; }
        }

        public void AddRentedUnitAndUpdateContract(RentedUnit rentedUnit)
        {
            RentedUnits = RentedUnits ?? new List<RentedUnit>();
            RentedUnits.Add(rentedUnit);
            if(GetActiveRentalContract()==null)
                RentedContract=new RentalContract();
            RentedContract.StartDate = rentedUnit.RentedDate;
            RentedContract.ExpiryDate = null;
            RentedContract.Charge = MonthlyCharge;
            RentedContract.PaymentPeriodId = 1;
            RentedContract.NoOfUnits = RentedUnits.Count;
            //RentedContract.AddAdjustmentPostForAddedRentedUnit(rentedUnit);
            rentedUnit.Stock.ProductLifeCycleId = (int) ProductLifeCycleStatus.Rented;
            rentedUnit.Stock.CustomerAccountId = this.CustomerAccountId;

        }


        public void UpdateRentedUnitAndUpdateContract(RentedUnit rentedUnit)
        {
            if (RentedUnits == null)
                AddRentedUnitAndUpdateContract(rentedUnit);
            else
            {
                rentedUnit.Stock.ProductLifeCycleId = (int)ProductLifeCycleStatus.Rented;
                rentedUnit.Stock.CustomerAccountId = this.CustomerAccountId;
                if (rentedUnit.OldAmount != 0 && rentedUnit.OldAmount != rentedUnit.Amount)
                {
                    //RentedContract.AddAdjustmentPostForRentedUnitPriceChange(rentedUnit);
                    RentedContract.Charge = MonthlyCharge;
                }
            }
        }

        public void RemoveRentedUnit(RentedUnit rentedUnit)
        {
            //RentedContract.AddAjustmentPostForRentedUnitRemoval(rentedUnit);
            rentedUnit.Stock.CustomerAccountId = null;
            if (RentedUnits.Count(c => c.RemovalDate == null) == 0)
            {
                RentedContract.Charge = MonthlyCharge;
                RentedContract.ExpiryDate = rentedUnit.RemovalDate;
                
            }
        }

        public int NoOfActiveUnits
        {
            get
            {
                return RentedUnits == null ? 0 : RentedUnits.Count(c => c.RemovalDate == null);
            }
        }
       
    }

    public class RentalAccountValidator : AbstractValidator<RentalAccount>
    {

        public RentalAccountValidator()
        {
            //RuleFor(x => x.RentedContract.PaymentPeriodId).GreaterThanOrEqualTo(0).WithMessage("Payment period must be selected");
            //RuleFor(x => x.RentedUnits).NotNull();

        }
    }
}