using HammondsIBMS_Domain.Values;

namespace HammondsIBMS_Domain.Entities.DocumentTemplates
{
    public static class DocumentTemplateEntityHelpers
    {
        public static string ReturnAddess(Address address)
        {
            return string.Format("{0}{1}{2}{3}{4}{5}",
                                 NextLineIfValue(address.AddressLine1),
                                 NextLineIfValue(address.AddressLine2),
                                 NextLineIfValue(address.AddressLine3),
                                 NextLineIfValue(address.City),
                                 NextLineIfValue(address.PostCode),
                                 NextLineIfValue(address.Country));
        }

        private static string NextLineIfValue(string value)
        {
            return string.IsNullOrEmpty(value) ? "" : value + "<br/>";
        }
    }
}