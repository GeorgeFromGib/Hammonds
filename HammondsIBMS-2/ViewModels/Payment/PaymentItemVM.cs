namespace HammondsIBMS_2.ViewModels.Payment
{
    public class PaymentItemVM
    {
        public int PaymentItemId { get; set; }

        public decimal Amount { get; set; }

        public string Type { get; set; }

        public string BillCycle { get; set; }

        public string Description { get; set; }

        public string AccountDescription { get; set; }
    }
}