using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HammondsIBMS_2.ViewModels.IBMSAccounts
{
    public class CustomerBalanceVM
    {
        [DisplayName("Current Balance")]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }
    }
}