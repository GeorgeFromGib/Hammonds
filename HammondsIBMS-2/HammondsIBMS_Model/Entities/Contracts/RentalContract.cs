using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HammondsIBMS_Domain.Entities.Accounts;
using HammondsIBMS_Domain.Entities.Invoicing;
using HammondsIBMS_Domain.Values;

namespace HammondsIBMS_Domain.Entities.Contracts
{
    public class RentalContract:Contract
    {

        [ScaffoldColumn(false)]
        public int CustomerAccountId { get; set; }
        [ScaffoldColumn(false)]
        public virtual CustomerAccount CustomerAccount { get; set; }

        [DisplayName("Payment Period Amount")]
        [DataType(DataType.Currency)]
        [UIHint("Money")]
        public override decimal PeriodPaymentAmount
        {
            get
            {
                try
                {
                    return (Charge * PaymentPeriod.PeriodInMonths);
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

        public int NoOfUnits { get; set; }

  


        //TODO Review. override
        protected  Contract CreateNewContractForPaymentPeriod(DateTime startDate,PaymentPeriod newPaymentPeriod)
        {
            return new RentalContract
                       {
                           Charge = this.Charge,
                           CustomerAccountId = this.CustomerAccountId,
                           LastBilled = new BillCycle(ConstantValues.MinDate),
                           StartDate = startDate,
                           PaymentPeriod = newPaymentPeriod
                       };
        }

        //TODO Review. override
        public  InvoiceAttachedContractItems PopulateInvoiceAttachedContractItems()
        {
            return new InvoiceAttachedContractItems {Amount = PeriodPaymentAmount, Description = "Rental ("+PaymentPeriod.Description+")"};
        }

       

        
    }
}