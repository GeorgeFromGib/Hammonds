using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;
using FluentValidation;
using HammondsIBMS_Domain.Entities.Accounts;
using HammondsIBMS_Domain.Entities.Invoicing;
using HammondsIBMS_Domain.Values;

namespace HammondsIBMS_Domain.Entities.Contracts
{
    public class Contract
    {
        public Contract()
        {
            LastBilled = new BillCycle();
        }

        [ScaffoldColumn(false)]
        public int ContractId { get; set; }

        [DisplayName("Start Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        //[ScaffoldColumn(false)]
        //public int CustomerAccountId { get; set; }
        //[ScaffoldColumn(false)]
        //public virtual CustomerAccount CustomerAccount { get; set; }

        [DisplayName("Contract Cost")]
        [DataType(DataType.Currency)]
        [UIHint("Money")]
        public virtual decimal Charge { get; set; }

        [DisplayName("Expiry Date")]
        [DataType(DataType.Date)]
        public virtual DateTime? ExpiryDate { get; set; }


        public bool ProRateRefunded { get; set; }

        [DisplayName("Payment Period Amount")]
        [DataType(DataType.Currency)]
        [UIHint("Money")]
        public virtual decimal PeriodPaymentAmount { get; set; }

        [DisplayName("Contract Status")]
        [UIHint("_ContractStatus")]
        public ContractStatus ContractStatus
        {
            get { return GetContractStatusOn(DateTime.Today); }
        }

        public bool IsExpiredToday()
        {
            return IsContractExpiredOnDate(DateTime.Today);
        }


        [DisplayName("Payment Period")]
        [UIHint("_PaymentPeriods")]
        public int PaymentPeriodId { get; set; }

        [ScaffoldColumn(false)]
        public virtual PaymentPeriod PaymentPeriod { get; set; }

        //public virtual List<AdjustmentPost> AdjustmentPosts { get; set; }

        public BillCycle LastBilled { get; set; }

        //TODO Review. Should transfer to service
        //public void ChangePaymentPeriod(PaymentPeriod newPaymentPeriod)
        //{
        //    if ( PaymentPeriodId != newPaymentPeriod.PaymentPeriodId)
        //    {
        //        if (LastBilled.Date != ConstantValues.MinDate)
        //        {
        //            //expire the contract at the end of the current term
        //            var nxtbillCycle = LastBilled.AddCycles(PaymentPeriod.PeriodInMonths);
        //            var expireDate = nxtbillCycle.Date.AddDays(-1);
        //            ExpiryDate = expireDate;

        //            //start new contract as from end of previous contract
        //            var newCont = CreateNewContractForPaymentPeriod(nxtbillCycle.Date, newPaymentPeriod);
        //            CustomerAccount.AddContract(newCont);

        //        }
        //        else
        //        {
        //            PaymentPeriod = newPaymentPeriod;
        //        }
                
        //    }
        //}

        //public abstract void ExpireContract();


        //protected abstract Contract CreateNewContractForPaymentPeriod(DateTime startDate,PaymentPeriod paymentPeriod);

        public ContractStatus GetContractStatusOn(DateTime dateTime)
        {
            if (ExpiryDate <= dateTime  || ExpiryDate==StartDate)
                return ContractStatus.CreateContractStatus(ContractStatus.ContractStatuses.Expired);
            if (StartDate > dateTime)
                return ContractStatus.CreateContractStatus(ContractStatus.ContractStatuses.Scheduled);
            
            return ContractStatus.CreateContractStatus(ContractStatus.ContractStatuses.Active);
        }

      

        public virtual bool IsBillableThisCycle(BillCycle billCycle)
        {
            return (LastBilled.Date == ConstantValues.MinDate && !IsContractScheduledBeyondDate(billCycle.Date)) || ((billCycle.BillCycleNo - LastBilled.BillCycleNo) == PaymentPeriod.PeriodInMonths && IsContractActiveOnDate(billCycle.Date));
        }

        
        public virtual bool IsContractActiveOnDate(DateTime dateTime)
        {
            return GetContractStatusOn(dateTime).ContractStatusId == (int)ContractStatus.ContractStatuses.Active;
        }

        public virtual bool IsContractExpiredOnDate(DateTime dateTime)
        {
            return GetContractStatusOn(dateTime).ContractStatusId == (int)ContractStatus.ContractStatuses.Expired;
        }

        public virtual bool IsContractScheduledBeyondDate(DateTime dateTime)
        {
            return GetContractStatusOn(dateTime).ContractStatusId == (int)ContractStatus.ContractStatuses.Scheduled;
        }

        //public abstract InvoiceAttachedContractItems PopulateInvoiceAttachedContractItems();

       

        

    }

    
}