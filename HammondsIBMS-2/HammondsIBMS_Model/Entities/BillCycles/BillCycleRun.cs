using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HammondsIBMS_Domain.Entities.Invoicing;

namespace HammondsIBMS_Domain.Entities.BillCycles
{
    public class BillCycleRun
    {
        public int BillCycleRunId { get; set; }
        public DateTime DateOfRun { get; set; }
        public BillCycle BillCycle { get; set; }
        public int NoOfCustomersInvoiced { get; set; }
        public decimal AmountInvoiced { get; set; }
    }
}
