using System.Web.Mvc;
using System.Web.UI.WebControls;
using HammondsIBMS_Application;
using HammondsIBMS_Domain.Entities.Misc;
using Telerik.Web.Mvc;
using Telerik.Web.Mvc.Extensions;

namespace HammondsIBMS_2.Controllers
{
    public class MiscController : IbmsBaseController
    {
        private readonly MiscService _miscService;

        public MiscController(MiscService miscService )
        {
            _miscService = miscService;
        }


        public ActionResult Index()
        {
            return View();
        }

        [GridAction]
        public ActionResult _MiscGridBind()
        {
            return View(new GridModel(_miscService.GetAll()));
        }

        public ActionResult _Edit(int id)
        {
            return PartialView(_miscService.GetById(id));
        }

        [HttpPost]
        public ActionResult _Edit(Misc mMisc)
        {
            var misc = _miscService.GetById(mMisc.MiscId);
            if (TryUpdateModel(misc))
            {
                if (ExecuteRepositoryAction(() => { _miscService.Update(misc); _miscService.CommitChanges(); }))
                    return ReturnJsonFormSuccess();
            }
            return PartialView(mMisc);
        }
    }
}
