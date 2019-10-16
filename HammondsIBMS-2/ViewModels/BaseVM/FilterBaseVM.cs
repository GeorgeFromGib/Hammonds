using System.ComponentModel.DataAnnotations;

namespace HammondsIBMS_2.ViewModels.BaseVM
{
    public class FilterBaseVM
    {
        [ScaffoldColumn(false)]
        public bool IsFiltered { get; set; }
    }
}