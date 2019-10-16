using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace HammondsIBMS_Domain.Entities.Accounts
{
    public class OneOffType
    {
        public int OneOffTypeId { get; set; }
        
        [DisplayName("Work type")]
        public string Description { get; set; }
    }

    public enum OneOffTypes
    {
        Sale=1,
        Work=2
    }
}
