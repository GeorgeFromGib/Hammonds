using System.Collections.Generic;

namespace HammondsIBMS_2.Helpers
{
    public class ContentType
    {
        private readonly MimeSettings _mimeSettings;
        Dictionary<string,string> _contentTypes=new Dictionary<string, string>(); 

        public ContentType(MimeSettings mimeSettings)
        {
            _mimeSettings = mimeSettings;
            SetupContentTypes();
        }

        private void SetupContentTypes()
        {
            foreach (MimeType mType in _mimeSettings.MimeTypes)
            {
                foreach (var extension in mType.Extensions.Split(';'))
                {
                    if(!string.IsNullOrEmpty(extension))
                        _contentTypes.Add(extension,mType.Type); 
                }
            }
        }

        public string GetContentTypeFromExtension(string extension)
        {
            var extn = extension.Substring(1, 1) == "." ? extension : "." + extension; //add pre-extension period if not supplied eg. .txt
            try
            {
                return _contentTypes[extn];
            }
            catch (KeyNotFoundException)
            {
                return "application/base64";
            }
            
        }
    }
}