using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HammondsIBMS_2.ViewModels.Collections
{
    public class DisplayCollectionVM
    {
        public int CustomerId { get; set; }
        public bool HasRentals { get; set; }

        public bool HasAccounts { get; set; }
    }
}