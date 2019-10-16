using HammondsIBMS_Domain.Entities.Documents;

namespace HammondsIBMS_Domain.Entities.Customers
{
    public class CustomerDocument:Document
    {
        public int CustomerDocumentId { get; set; }
        public int CustomerId { get; set; }

    }
}
