using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace HammondsIBMS_Domain.Entities.AccountTransactions
{
    public enum AccountTransactionType
    {
        OneOff = 1,
        Payment = 2,
        [Description("Rental Payment")]
        RentalPayment = 3,
        Purchase = 4,
        Deposit = 5,
        Rental = 6,
        [Description("Deposit Return")]
        DepositReturn = 7,
        Refund,
        Service
    }
}
