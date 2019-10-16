namespace HammondsIBMS_2.ViewModels.BillCycleRun
{
    public class NewBillCycleRunVM
    {
        public bool IsError { get; set; }
        public string Message { get; set; }
        public int NoOfCustomersToInvoice { get; set; }
        public string BillCycle { get; set; }
    }
}