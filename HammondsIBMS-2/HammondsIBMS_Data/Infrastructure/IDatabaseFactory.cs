using System;

namespace HammondsIBMS_Data.Infrastructure
{
    public interface IDatabaseFactory : IDisposable
    {
        HammondsIBMSContext Get();
    }
}
