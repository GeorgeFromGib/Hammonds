using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace HammondsIBMS_Domain.Entities.AccountTransactions
{
    public enum AccountTransactionType
    {
        [Description("One Off")]
        OneOff = 1,
        Payment = 2,
        [Description("Rental Payment")]
        Purchase = 4,
        Deposit = 5,
        Rental = 6,
        [Description("Deposit Return")]
        DepositReturn = 7,
        Refund,
        Service
    }
}
