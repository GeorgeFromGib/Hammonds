using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using FluentValidation.Attributes;
using HammondsIBMS_Domain.Entities.Invoicing;
using HammondsIBMS_Domain.Values;

namespace HammondsIBMS_Domain.Entities.Contracts
{
    [Validator(typeof (ServiceContractValidator))]
    public class ServiceContract : Contract
    {

        [DisplayName("Contract Type")]
        [UIHint("_ContractTypes")]
        public int ContractTypeId { get; set; }

        [ScaffoldColumn(false)]
        public virtual ContractType ContractType { get; set; }

        [DisplayName("Contract Length (months)")]
        [UIHint("_NumericInt")]
        public int ContractLengthInMonths { get; set; }

        public void ChangeContractType(ContractType contractType)
        {
            ContractType = contractType;
            if (contractType != null)
            {
                ContractLengthInMonths = contractType.PeriodInMonths;
                Charge = contractType.NormalTermAmount;
            }
        }

        [DisplayName("Days to Expiry")]
        [UIHint("_TimeSpanFormat")]
        public int NoOfDaysLeft
        {
            get
            {

                var dl = ExpiryDate==null?0:(ExpiryDate.Value - DateTime.Now).Days;
                return dl >= 0 ? dl : 0;
            }
        }

        [DisplayName("Expiry")]
        public string TimeToExpiry
        {
            get
            {
                var noOfDays = NoOfDaysLeft;
                if (noOfDays > 365)
                {
                    int yrs = noOfDays/365;
                    int mth = (int) ((noOfDays%365)/ConstantValues.AvgDaysPerMonth);
                    return (yrs + "y " + mth + "m");
                }
                else
                {
                    int mth = (int) (noOfDays/ConstantValues.AvgDaysPerMonth);
                    int day = (int) (noOfDays%ConstantValues.AvgDaysPerMonth);
                    return (mth + "m " + day + "d");
                }
            }
        }

        [DisplayName("Payment Period Amount")]
        [DataType(DataType.Currency)]
        [UIHint("Money")]
        public override decimal PeriodPaymentAmount
        {
            get
            {
                try
                {
                    return (Charge*PaymentPeriod.PeriodInMonths)/ContractLengthInMonths;
                }
                catch (DivideByZeroException)
                {
                    return 0;
                }
                catch (NullReferenceException)
                {
                    return 0;
                }

            }
        }

        public bool RequiresInvoicingProErrata { get; set; }


        public override bool IsBillableThisCycle(BillCycle billCycle)
        {
            return base.IsBillableThisCycle(billCycle) && !ContractType.HideFromInvoice;
        }


    }

    public class ServiceContractValidator : AbstractValidator<ServiceContract>
    {
        public ServiceContractValidator()
        {

            RuleFor(x => x.PaymentPeriodId).GreaterThanOrEqualTo(0).WithMessage("Payment period must be selected");
            RuleFor(x => x.ContractTypeId).GreaterThanOrEqualTo(0).WithMessage("Contract Type must be selected");
            RuleFor(x => x.ContractLengthInMonths).GreaterThanOrEqualTo(1).WithMessage(
                "Contract length can not be less than 1 month");
            RuleFor(x => x.ContractLengthInMonths).GreaterThanOrEqualTo(x => x.PaymentPeriod.PeriodInMonths).WithMessage(
                "Contract length can not be less than Payment Period");
            RuleFor(x => x.ExpiryDate).GreaterThanOrEqualTo(x => x.StartDate).When(x=>x.ExpiryDate!=ConstantValues.MinDate).WithMessage(
                "Expiry Date can not be less that Start Date.");

        }
    }
}