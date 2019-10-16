using System.Collections.Generic;

namespace HammondsIBMS_2.ViewModels.Menu
{
    public class MenuVM
    {
        public int Index { get; set; }
        public string Title { get; set; }
        public string Action { get; set; }
        public bool Active { get; set; }
        public MenuVMType MenuType { get; set; }
        public MenuVM Parent { get; set; }
        public List<MenuVM> SubMenu { get; set; }
        
    }

    public enum MenuVMType
    {
        Item,
        Divider
    }
}