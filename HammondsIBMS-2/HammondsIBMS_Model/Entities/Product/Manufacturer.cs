using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HammondsIBMS_Domain.BaseInterfaces;

namespace HammondsIBMS_Domain.Model.Product
{
    public partial class Manufacturer
    {
        public int ManufacturerId { get; set; }
       
        //[Required]
        public string Name { get; set; }

        public bool Retired { get; set; }

        //[DisplayName("Web Site")]
        //public string HyperLink { get; set; }
    }
}
