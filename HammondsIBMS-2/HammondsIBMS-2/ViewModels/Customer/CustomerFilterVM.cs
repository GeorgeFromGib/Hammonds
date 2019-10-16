using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using HammondsIBMS_2.ViewModels.BaseVM;

namespace HammondsIBMS_2.ViewModels.Customer
{
    public class CustomerFilterVM:FilterBaseVM
    {
        [DisplayName("")]
        public string SearchString { get; set; }
    }
}