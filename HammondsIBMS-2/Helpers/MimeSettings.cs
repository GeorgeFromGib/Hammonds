using System.Configuration;

namespace HammondsIBMS_2.Helpers
{
    public class MimeSettings:ConfigurationSection
    {
        [ConfigurationProperty("MimeTypes")]
        [ConfigurationCollection(typeof(MimeTypeCollection))]
        public MimeTypeCollection MimeTypes
        {
            get { return this["MimeTypes"] as MimeTypeCollection; }
        }
    }

    public class MimeType : ConfigurationElement
    {
        [ConfigurationProperty("Type")]
        public string Type
        {
            get { return this["Type"] as string; }

        }

        [ConfigurationProperty("Extensions")]
        public string Extensions
        {
            get { return this["Extensions"] as string; }
        }
    }

    public class MimeTypeCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new MimeType();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((MimeType)element).Type;
        }

        public MimeType this[int index]
        {
            get { return (MimeType) BaseGet(index); }
        }
    }

}