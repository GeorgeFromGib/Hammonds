using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;

//using DataAnnotationsExtensions;

namespace HammondsIBMS_Domain.Values
{
    public class ContactInfo
    {
        [DisplayName("Telephone")]
        [RegularExpression(@"^\+*([0-9\s\-]){10,}$", ErrorMessage = "Incorrect telephone number format")]
        public string Tel { get; set; }

        [RegularExpression(@"^\+*([0-9\s\-]){10,}$", ErrorMessage = "Incorrect telephone number format")]
        public string Mobile { get; set; }

        [Email]
        public string Email { get; set; }

        [RegularExpression(@"^\+*([0-9\s\-]){10,}$", ErrorMessage = "Incorrect telephone number format")]
        public string Fax { get; set; }

        [DisplayName("2nd Telephone")]
        [RegularExpression(@"^\+*([0-9\s\-]){10,}$", ErrorMessage = "Incorrect telephone number format")]
        public string Tel2 { get; set; }

        [DisplayName("2nd Mobile")]
        [RegularExpression(@"^\+*([0-9\s\-]){10,}$", ErrorMessage = "Incorrect telephone number format")]
        public string Mobile2 { get; set; }

        [DisplayName("2nd Email")]
        [Email]
        public string Email2 { get; set; }

        [DisplayName("2nd Fax")]
        [RegularExpression(@"^\+*([0-9\s\-]){10,}$", ErrorMessage = "Incorrect telephone number format")]
        public string Fax2 { get; set; }

        [RegularExpression(@"^(https?:\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \.-]*)*\/?$",ErrorMessage="Incorrect URL")]
        [UIHint("_Url")]
        public string Url { get; set; }
    }
}