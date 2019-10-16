using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FluentValidation.Attributes;
using HammondsIBMS_Domain.Entities.Accounts;
using HammondsIBMS_Domain.Model.Stock;
using HammondsIBMS_Domain.Values;

namespace HammondsIBMS_Domain.Entities.Contracts
{
 

    public static class ContractUnitFactory
    {
        public static CustomerAccount CreateContractUnit(AccountType accountType,int customerId,Stock.Stock stock,DateTime startDate,decimal rentalAmount,IEnumerable<ContractType> contractTypes,int paymentPeriodId  ) 
        {
            switch (accountType)
            {
                case AccountType.Purchase:
                    var contractUnitPurchase = new PurchaseAccount {CustomerId = customerId};
                    GetDefaultContracts(contractTypes, startDate).ForEach(contractUnitPurchase.AddContract);
                    return contractUnitPurchase;

                case AccountType.Rent:
                    var cur = new RentalAccount
                                  {
                                      CustomerId = customerId,
                                      RentedContract = new RentalContract{StartDate = startDate,PaymentPeriodId = paymentPeriodId},
                                      RequiresInvoicingProErrata = true,
                                  };
                    if(stock!=null)
                        cur.AddRentedUnitAndUpdateContract(new RentedUnit{StockId = stock.StockId,Stock=stock,Amount = rentalAmount,RentedDate = startDate});
                    return cur;
                default:
                    throw new ApplicationException("Unknown Contract type supplied to ContractUnitFactory");
            }
        }

        private static List<Contract> GetDefaultContracts(IEnumerable<ContractType> contractTypes,DateTime startDate)
        {
            var dftConTypes = contractTypes.Where(c => c.PurchaseDefault == true).ToList();
            return new List<Contract>(dftConTypes.Select(c => new ServiceContract
                                                                  {
                                                                      ContractTypeId = c.ContractTypeId, 
                                                                      PaymentPeriodId = 1, 
                                                                      ContractLengthInMonths = c.PeriodInMonths, 
                                                                      StartDate = startDate,
                                                                      ExpiryDate = startDate.AddDays(c.PeriodInMonths*ConstantValues.AvgDaysPerMonth),
                                                                      RequiresInvoicingProErrata = true
                                                                  }).ToList());
        }
    }
}

