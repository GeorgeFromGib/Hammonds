using System.ComponentModel.DataAnnotations;

namespace HammondsIBMS_Domain.Entities.Documents
{
    public class Document
    {
        public int DocumentId { get; set; }

        [Required]
        public string Title { get; set; }

        public string FileName { get; set; }

        public byte[] ImageBytes { get; set; }

        public string ContentType { get; set; }
    }
}