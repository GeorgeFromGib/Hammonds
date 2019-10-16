using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;

namespace HammondsIBMS_2.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult _BuildHistory()
        {
            var xmlser = new XmlSerializer(typeof (releases));
            releases relDta;
            using (var fs = new FileStream(Server.MapPath("~/ReleaseTracker/ReleasesData.xml"), FileMode.Open))
            {
                relDta =(xmlser.Deserialize(fs)as releases);
            }
            return PartialView(relDta);
        }
    }
}
