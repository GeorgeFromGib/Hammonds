using System.ComponentModel.DataAnnotations;

namespace HammondsIBMS_2.ViewModels.Documents
{
    public class DocumentVM
    {
        public int DocumentId { get; set; }

        [Required]
        public string Title { get; set; }

        public string FilePath { get; set; }
    }
}