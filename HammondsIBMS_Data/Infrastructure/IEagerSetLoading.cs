using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HammondsIBMS_Data.Infrastructure
{
    public interface IEagerSetLoading<T> where T:class
    {
        IEnumerable<T> GetAllEager();
    }
}
