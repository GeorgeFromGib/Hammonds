using System.Data.Entity;
using HammondsIBMS_2.DataScaffolding;
using HammondsIBMS_Data;

namespace HammondsIBMS_2.Setup
{
    public static class DataReferenceSetup
    {
        public static DbContext Setup(bool loadTestData=true)
        {
            DropCreateDatabaseIfModelChanges<HammondsIBMSContext> strategy = null;
            if (loadTestData)
                strategy = new InitialiseHammondsDb();
            else
                strategy = new InitialiseHammondsNoTestDataDb();

            Database.SetInitializer<HammondsIBMSContext>(strategy);
            //Database.SetInitializer<HammondsIBMS_DB>(new CreateDatabaseIfNotExists<HammondsIBMS_DB>());
            return new HammondsIBMSContext();
        }

    }
}