using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HammondsIBMS_Domain.BaseInterfaces
{
    public interface IDomainEvent
    {
        DateTime OccurredOn();
    }
}
