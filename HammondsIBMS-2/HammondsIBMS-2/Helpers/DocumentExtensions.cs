using System.IO;
using HammondsIBMS_Domain.Entities.Documents;

namespace HammondsIBMS_2.Helpers
{
    public static class DocumentExtensions
    {
        public static Document LoadFile(this Document document, string file)
        {
            using (var strm = File.Open(file, FileMode.Open))
            {
                document.FileName = Path.GetFileName(file);
                document.ContentType = Path.GetExtension(file);
                return document.LoadFile(strm);
            }
        }

        public static Document LoadFile(this Document document, System.IO.Stream stream)
        {
            byte[] binData;
            var length = (int)stream.Length;
            binData = new byte[length];
            stream.Read(binData, 0, length);

            document.ImageBytes = binData;

            return document;
        }
    }
}