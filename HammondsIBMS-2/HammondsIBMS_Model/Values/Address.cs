using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HammondsIBMS_Domain.Values
{
    public class Address
    {
        //[Required]
        [DisplayName("Address Line 1")]
        public string AddressLine1 { get; set; }
        [DisplayName("Address Line 2")]
        public string AddressLine2 { get; set; }
        [DisplayName("Address Line 3")]
        public string AddressLine3 { get; set; }
        public string City { get; set; }
        [DisplayName("Post Code")]
        public string PostCode { get; set; }
        public string Country { get; set; }

        [DisplayName("Address")]
        public string FullAddress
        {
            get { return string.Format("{0}, {1}, {2}, {3}, {4}", AddressLine1, AddressLine2, AddressLine3, City, Country); }
        }
    }


  
}
