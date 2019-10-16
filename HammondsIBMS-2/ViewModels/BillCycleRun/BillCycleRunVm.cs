using System;

namespace HammondsIBMS_2.ViewModels.BillCycleRun
{
    public class BillCycleRunVM
    {
        public int BillCycleRunId { get; set; }
        public DateTime DateOfRun { get; set; }
        public string BillCycle { get; set; }
        public int NoOfCustomersInvoiced { get; set; }
        public decimal AmountInvoiced { get; set; }
    }
}