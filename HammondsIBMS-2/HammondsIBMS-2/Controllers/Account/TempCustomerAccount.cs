using HammondsIBMS_2.Helpers;
using HammondsIBMS_Domain.Entities.Accounts;

namespace HammondsIBMS_2.Controllers
{
    public static class TempCustomerAccount
    {
        public static CustomerAccount TempAccount
        {
            get { return TempRepository<CustomerAccount>.Entity; }
            set { TempRepository<CustomerAccount>.Entity = value; }
        }
    }
}