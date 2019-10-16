using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HammondsIBMS_Domain.Entities.Accounts;
using HammondsIBMS_Domain.Entities.Contracts;

namespace HammondsIBMS_2.ViewModels
{
    public class AddOneOffItemsVM
    {
        public List<ItemCharge> AvailableOneOfCharges { get; set; }

        public List<OneOffItem> OneOffItems { get; set; }
    }
}