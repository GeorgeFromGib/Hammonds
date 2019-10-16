using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HammondsIBMS_2.ViewModels.Accounts
{
    public class DeleteOneOffItemsPromptVM : PromptVM
    {

        [DisplayName("One-Off Item to remove")]
        public string Message { get; set; }

        [DisplayName("Refund?")]
        public bool Refunded { get; set; }

        [DisplayName("Amount")]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }
    }
}