using System.Data.Entity;
using HammondsIBMS_Data;

namespace HammondsIBMS_2.DataScaffolding
{

    public class InitialiseHammondsDb : DropCreateDatabaseIfModelChanges<HammondsIBMSContext>
    {
        protected override void Seed(HammondsIBMSContext context)
        {
            DbPopulator.Populate(context);
        }
    }

    public class InitialiseHammondsNoTestDataDb : DropCreateDatabaseIfModelChanges<HammondsIBMSContext>
    {
        protected override void Seed(HammondsIBMSContext context)
        {
            DbPopulator.Populate(context,false);
        }
    }
}