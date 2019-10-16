using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HammondsIBMS_2.ViewModels.Contract
{
    public class ExpireServiceContractVM
    {
        public int ContractId { get; set; }

        [DisplayName("Start Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DisplayName("Expiry Date")]
        [DataType(DataType.Date)]
        public DateTime ExpiryDate { get; set; }

        [DisplayName("Contract Description")]
        public string ContractDescription { get; set; }


    }
}