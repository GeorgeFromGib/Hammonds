using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HammondsIBMS_Application;
using HammondsIBMS_Data.Repositories;
using HammondsIBMS_Domain.Entities.Contracts;
using Telerik.Web.Mvc;

namespace HammondsIBMS_2.Controllers
{
    public class ItemChargeController : IbmsBaseController
    {
        private readonly ItemChargeService _itemChargeService;

        public ItemChargeController(ItemChargeService itemChargeService)
        {
            _itemChargeService = itemChargeService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [GridAction]
        public ActionResult ItemChargeGridBind()
        {
            var charges = _itemChargeService.GetItemCharges();
            return View(new GridModel(charges));
        }

        public ActionResult _AddItemCharge()
        {
            var itemCharge = new ItemCharge();
            return PartialView("_EditItemCharge", itemCharge);
        }

        [HttpPost]
        public ActionResult _AddItemCharge(ItemCharge mItemCharge)
        {
            if(ModelState.IsValid)
            {
                if (ExecuteRepositoryAction(() => { _itemChargeService.AddItemCharge(mItemCharge); _itemChargeService.CommitChanges(); }))
                    return ReturnJsonFormSuccess();
            }
            return PartialView("_EditItemCharge",mItemCharge);
        }

        public ActionResult _EditItemCharge(int id)
        {
            var itemCharge = _itemChargeService.GetItemCharge(id);
            return PartialView(itemCharge);
        }

        [HttpPost]
        public ActionResult _EditItemCharge(ItemCharge mItemCharge)
        {
            if(ModelState.IsValid)
            {
                var itemCharge = _itemChargeService.GetItemCharge(mItemCharge.ItemChargeId);
                if(TryUpdateModel(itemCharge))
                {
                    if (ExecuteRepositoryAction(() => { _itemChargeService.UpdateItemCharge(itemCharge); _itemChargeService.CommitChanges(); }))
                        return ReturnJsonFormSuccess();
                }
            }
            return PartialView(mItemCharge);
        }
    }
}
