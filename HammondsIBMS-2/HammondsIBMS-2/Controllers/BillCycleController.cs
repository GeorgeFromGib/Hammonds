using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using HammondsIBMS_2.Setup;
using HammondsIBMS_2.ViewModels.BillCycleRun;
using HammondsIBMS_Application;
using HammondsIBMS_Domain.Entities.BillCycles;
using Telerik.Web.Mvc;

namespace HammondsIBMS_2.Controllers
{
    public class LongRunningTaskParamters
    {
        public bool IsRunning { get; set; }
        public int Count { get; set; }
    }

    public class BillCycleController : IbmsBaseController
    {
        private static Dictionary<Guid, LongRunningTaskParamters> tasks = new Dictionary<Guid, LongRunningTaskParamters>(); 

        private readonly BillCycleService _billCycleService;


        public BillCycleController(BillCycleService billCycleService)
        {
            _billCycleService = billCycleService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [GridAction]
        public ActionResult _BillCycleHistoryGridBind()
        {
            var vm=AutoMapperSetup.MapList<BillCycleRun,BillCycleRunVM>(_billCycleService.GetBillCycleHistory());
            return View(new GridModel(vm));
        }

        public ActionResult _LastBillCycle()
        {
            return PartialView("_LastBillCycle",_billCycleService.GetLastBillCycle());
        }

        public ActionResult _NewBillCycle()
        {
            
            var lastBillCycle = _billCycleService.GetLastBillCycle();
            var daysElapsed = (DateTime.Today - lastBillCycle.Date).TotalDays;
            var minimumNoOfDaysBetweenBillingCycles = _billCycleService.GetMinimumNoOfDaysBetweenBillingCycles();
            var daysRemaining = minimumNoOfDaysBetweenBillingCycles-daysElapsed;
            var nextBillingCycle = lastBillCycle.AddCycles(1);

            var nbc = new NewBillCycleRunVM { IsError = true, BillCycle = nextBillingCycle.ToString() };

           
            if (daysElapsed < minimumNoOfDaysBetweenBillingCycles)
            {
                nbc.Message = string.Format("Number of days between Billing Cycles has yet not elapsed! ( {0} days remaining )",daysRemaining);
            }
            else
            {
                nbc.IsError = false;
                nbc.Message = string.Format("Do you wish to start the {0} Billing Cycle run?",
                                            nextBillingCycle.ToString());
                nbc.NoOfCustomersToInvoice = _billCycleService.GetNoOfCustomersToInvoice();
            }

            return PartialView(nbc);

        }


        public ActionResult _StartBillingCycle()
        {
            var taskId = Guid.NewGuid();
            tasks.Add(taskId,new LongRunningTaskParamters{Count = 0,IsRunning = true});

            Task.Factory.StartNew(()=>RunBillingCycle(_billCycleService,taskId));

            return Json(taskId, JsonRequestBehavior.AllowGet);
        }

        private void RunBillingCycle(BillCycleService billCycleService,Guid taskId)
        {
            var nextBillingCycle = billCycleService.GetLastBillCycle().AddCycles(1);
            billCycleService.BillCycleRunChanged +=
                delegate(object o, bool isRunning, int noOfCustomersInvoiced)
                    {
                        tasks[taskId].Count = noOfCustomersInvoiced;
                        tasks[taskId].IsRunning = isRunning;
                    };
            billCycleService.ExecuteBillCycleRun(nextBillingCycle);
            billCycleService.CommitChanges();
            tasks.Remove(taskId);

        }

        public ActionResult _ReadBillingCycleRunningStatus(Guid id)
        {
            var lrtp = tasks.ContainsKey(id) ? tasks[id] : new LongRunningTaskParamters{Count = 100,IsRunning = false};
            return Json(new { count = lrtp.Count, isRunning = lrtp.IsRunning }, JsonRequestBehavior.AllowGet);
        }
    }
}
