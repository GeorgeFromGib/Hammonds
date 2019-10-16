using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HammondsIBMS_2.ViewModels.Manufacturer
{
    public class DeleteManufacturerVM
    {
        public int ManufacturerId { get; set; }
        public string DeleteMessage { get; set; }

        public string Prompt { get; set; }

        public bool MakeObsolete { get; set; }
    }
}