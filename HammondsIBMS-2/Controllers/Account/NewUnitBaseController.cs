using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HammondsIBMS_2.Helpers;
using HammondsIBMS_2.ViewModels;
using HammondsIBMS_Application;
using HammondsIBMS_Domain.Entities.Accounts;
using HammondsIBMS_Domain.Entities.Contracts;
using Telerik.Web.Mvc;

namespace HammondsIBMS_2.Controllers.Account
{
    public abstract class NewUnitBaseController : IbmsBaseController
    {
        protected readonly CustomerAccountService _accountService;
        protected readonly ItemChargeService _itemChargeService;
        protected readonly CustomerService _customerService;

        protected NewUnitBaseController(CustomerAccountService accountService, ItemChargeService itemChargeService,CustomerService customerService)
        {
            _accountService = accountService;
            _itemChargeService = itemChargeService;
            _customerService = customerService;
        }

        public abstract ActionResult AddUnitBasket(int customerAccountId);

        public abstract ActionResult EditBasketForAccount(int customerAccountId);

        protected int BasketId
        {
            get
            {
                var entity = Session["_basketId"] as int?;
                return entity ?? 0;
            }
            set { Session["_basketId"] = value; }
        }

        protected Basket GetBasket()
        {
            return _accountService.GetBasket(BasketId);
        }

        protected ActionResult RemoveBasketAndReturnToAccountPage(bool isCancel)
        {
            var basket = GetBasket();

            var customerId = basket.CustomerAccount.CustomerId;
            var customerAccountId = basket.CustomerAccountId;

            _accountService.DeleteBasket(basket, isCancel);
            _accountService.CommitChanges();

            return RedirectToAction("Edit", "Customer",
                new {id = customerId, moveOnToEditAccount = customerAccountId});
        }


        

        #region TempOneOffItems

        public ActionResult _AvailableOneOffCharges()
        {
            var oneOffItems =
                _itemChargeService.GetItemCharges()
                                  .Select(
                                      c =>
                                      new
                                      {
                                          c.ItemChargeId,
                                          Description = c.Description + " " + c.Amount.ToString("C")
                                      }).ToList();
            oneOffItems = AddAllSelector(new { ItemChargeId = -1, Description = "- Select To Add -" },
                                         oneOffItems);
            return new JsonResult { Data = new SelectList(oneOffItems, "ItemChargeId", "Description") };
        }


        [GridAction]
        public ActionResult _DisplayItemCharges()
        {
            var itemCharges = GetBasket().OneOffItems;
            return View(new GridModel(itemCharges));
        }



        public ActionResult _EditItemCharge(int id)
        {
            var oneOffItem = GetBasket().OneOffItems.Single(c => c.OneOffItemId == id);
            return PartialView(oneOffItem);
        }

        [HttpPost]
        public ActionResult _EditItemCharge(OneOffItem mOneOffItem)
        {
            var oneOffItem = GetBasket().OneOffItems.Single(c => c.OneOffItemId == mOneOffItem.OneOffItemId);
            if (TryUpdateModel(oneOffItem))
            {
                return ReturnJsonFormSuccess();
            }
            return PartialView(mOneOffItem);
        }

        public ActionResult _DeleteItemCharge(int id)
        {
            var itemCharge = GetBasket().OneOffItems.Single(c => c.OneOffItemId == id);
            var promptVm = new PromptVM(id, new DialogPrompt("Do you wish to remove Item Charge '" + itemCharge.Description, PromptMessageAlertLevel.Warning));
            return PartialView("_PromptDialog", promptVm);
        }

        [HttpPost]
        public ActionResult _DeleteItemCharge(PromptVM promptVm)
        {
            GetBasket().OneOffItems.RemoveAll(c => c.OneOffItemId == promptVm.RecordIndex);
            return ReturnJsonFormSuccess();
        }

        [HttpPost]
        public ActionResult _AddOneOffCharge(int id, int qty = 1)
        {
            var presetOneOfItem = _itemChargeService.GetItemCharge(id);
            var oneOfItem = CreateOneOfItemFromPresetOneOffCharges(presetOneOfItem);
            oneOfItem.OneOffItemId = GetBasket().OneOffItems.NextIndex(c => c.OneOffItemId);
            oneOfItem.Quantity = qty;

            var basket = GetBasket();
            basket.OneOffItems.Add(oneOfItem);
            UpdateBasket(basket);

            return ReturnJsonFormSuccess();
        }


        protected List<OneOffItem> InitiateOneOffItems(Func<ItemCharge, bool> where, int quantity = 1)
        {
            var oneOffItems = new List<OneOffItem>();
            foreach (var itemCharge in _itemChargeService.GetItemCharges().Where(where))
            {
                var ofi = CreateOneOfItemFromPresetOneOffCharges(itemCharge, quantity);
                ofi.OneOffItemId = oneOffItems.NextIndex(c => c.OneOffItemId);
                oneOffItems.Add(ofi);
            }

            return oneOffItems;
        }

        private static OneOffItem CreateOneOfItemFromPresetOneOffCharges(ItemCharge itemCharge, int quantity = 1)
        {
            return new OneOffItem
            {
                Date = DateTime.Today,
                Description = itemCharge.Description,
                Charge = itemCharge.Amount,
                Quantity = quantity,
                OneOffTypeId = (int)OneOffTypes.Work
            };
        }

        #endregion

        protected void UpdateBasket(Basket basket)
        {
            _accountService.UpdateBasket(basket);
            _accountService.CommitChanges();
        }
    }
}
