using System.Web.Mvc;
using HammondsIBMS_2.ViewModels.BaseVM;

namespace HammondsIBMS_2.Controllers
{
    public abstract class FilterController<M>:IbmsBaseController where M:FilterBaseVM
    {
        private readonly string _sessionId;
        
        protected abstract M ResetFilterVM();
        protected abstract void AddSelectors();
        public M FilterVM
        {
            get
            {
                return Session[_sessionId] as M;
            }
            set { Session[_sessionId] = value; }
        }

        protected FilterController()
        {
            _sessionId = typeof(M).Name;
        }

        [OutputCache(Duration = 0)]
        public ActionResult _FilterReset()
        {
            AddSelectors();
            FilterVM = ResetFilterVM();
            return PartialView("_Filter", FilterVM);
        }

        public ActionResult _Filter()
        {
            AddSelectors();
            var vm = FilterVM ?? ResetFilterVM();
            return PartialView(vm);
        }

        [HttpPost]
        public ActionResult _Filter(M mFilterVm)
        {
            mFilterVm.IsFiltered = true;
            FilterVM = mFilterVm;
            return ReturnJsonFormSuccess();
        }
    }
}