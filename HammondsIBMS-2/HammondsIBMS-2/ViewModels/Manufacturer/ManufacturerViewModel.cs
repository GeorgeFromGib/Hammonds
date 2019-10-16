using System.ComponentModel.DataAnnotations;

namespace HammondsIBMS_2.ViewModels.Manufacturer
{

    public class ManufacturerVM 
    {
        public int ManufacturerId { get; set; }
        
        [Required]
        public string Name { get; set; }

        public bool Retired { get; set; }


    }
}