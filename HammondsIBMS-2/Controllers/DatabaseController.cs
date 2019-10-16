using System.Web.Mvc;
using HammondsIBMS_2.DataScaffolding;
using HammondsIBMS_2.Setup;
using HammondsIBMS_Data;

namespace HammondsIBMS.Controllers
{
    public class DatabaseController : Controller
    {
       

        // GET: /Database/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Repopulate()
        {
//#if DEBUG
            var ctx = DataReferenceSetup.Setup();
            ctx.Database.Delete();
            ctx.Database.Initialize(true);
//#else
//            // In test production **************************
//            var ctx = new HammondsIBMSContext();
//            DbPopulator.ClearTables(ctx);
//            DbPopulator.Populate(ctx);
//            ctx.SaveChanges();
//            // ************************************************
//#endif


            ViewBag.Action = "Re-populated";
            //DataReferenceSetup.Setup();
            return PartialView("_Repopulate");
        }

        public ActionResult Reset()
        {

            var ctx = DataReferenceSetup.Setup(false);
            ctx.Database.Delete();
            ctx.Database.Initialize(true);


            ViewBag.Action = "Reset";
            return PartialView("_Repopulate");
        }



    }
}
