using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HammondsIBMS_Domain.Entities.Stock
{
    public class InvoiceStatus
    {
        public int InvoiceStatusId { get; set; }
        public string StatusDescription { get; set; }
    }

    public enum InvoiceStatusEnum
    {
        Pending=1,
        Attached=2,
        Closed=3                           
    }
}
