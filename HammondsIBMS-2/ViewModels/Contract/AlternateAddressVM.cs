using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HammondsIBMS_Domain.Values;

namespace HammondsIBMS_2.ViewModels.Contract
{
    public class AlternateAddressVM
    {
        public int CustomerAccountId { get; set; }
        public Address AlternateAddress { get; set; }
        
        [DisplayName("Instructions")]
        [DataType(DataType.MultilineText)]
        public string AlternateAddressInstructions { get; set; }
    }
}