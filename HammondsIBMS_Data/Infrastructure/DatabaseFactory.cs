namespace HammondsIBMS_Data.Infrastructure
{
public class DatabaseFactory : Disposable, IDatabaseFactory
{
    private HammondsIBMSContext dataContext;
    public HammondsIBMSContext Get()
    {
        return dataContext ?? (dataContext = new HammondsIBMSContext());
    }
    protected override void DisposeCore()
    {
        if (dataContext != null)
            dataContext.Dispose();
    }
}
}
