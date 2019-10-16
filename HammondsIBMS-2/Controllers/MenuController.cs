using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HammondsIBMS_2.ViewModels;
using HammondsIBMS_2.ViewModels.Menu;
using Telerik.Web.Mvc;
using SiteMapNode = Telerik.Web.Mvc.SiteMapNode;

namespace HammondsIBMS_2.Controllers
{
    public class MenuController : Controller
    {
        //
        // GET: /Menu/
        

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _MainMenu()
        {
            var nodes = GetSiteMap("Main");
            var vm = new List<MenuVM>();
            var selIdx = MenuSelectionStore;
            var idx = 0;
            foreach (var sm in nodes)
            {
                var mnu = new MenuVM {Title = sm.Title, Action = sm.Url, Index = idx, Active = (selIdx == idx),SubMenu = new List<MenuVM>()};
                mnu.SubMenu.AddRange(sm.ChildNodes.Select(m => new MenuVM { Title = m.Title, 
                    Action =  UrlHelper.GenerateContentUrl(m.Url, HttpContext), 
                    Parent = mnu,
                    MenuType = (m.Title == "DIVIDER" ? MenuVMType.Divider : MenuVMType.Item) }));
                vm.Add(mnu);
                idx++;
            }

            return PartialView(vm);
        }

        private static IList<SiteMapNode> GetSiteMap(string name)
        {
            var val = SiteMapManager.SiteMaps[name];
            var val2 = val.RootNode.ChildNodes;
            return val2;
        }

        public ActionResult _SideMenu(int id)
        {
            var vm = new SideMenuVM {MenuVMs = new List<MenuVM>()};

            var nodes = GetSiteMap("Main");
            var pId = MenuSelectionStore;
            if (pId == -1 && id > -1) pId = id;
            if (pId > -1)
            {
                vm.Header = nodes[pId].Title;
                var childNodes = nodes[pId].ChildNodes;
                var selIdx = SideMenuSelectionStore;
                var idx = 0;
                foreach (var sm in childNodes)
                {
                    vm.MenuVMs.Add(new MenuVM {Title = sm.Title, Action = UrlHelper.GenerateContentUrl(sm.Url,HttpContext)  , Index = idx, Active = (selIdx == idx),MenuType = (sm.Title=="DIVIDER"?MenuVMType.Divider : MenuVMType.Item)});
                    idx++;
                }
            }
            return PartialView(vm);
        }

        [HttpPost]
        public ActionResult _MainMenuSelection(int id)
        {
            MenuSelectionStore = id;
            return null;
        }

        [HttpPost]
        public ActionResult _SideMenuSelection(int id)
        {
            SideMenuSelectionStore = id;
            return null;
        }

        private int MenuSelectionStore
        {
            get
            {
                var val = Session["_mainMenuStore"];
                return (int) (val??-1);
            }
            set { Session["_mainMenuStore"] = value; }
        }

        private int SideMenuSelectionStore
        {
            get { return (int)(Session["_sideMenuStore"] ?? -1); }
            set { Session["_sideMenuStore"] = value; }
        }

    }
}
