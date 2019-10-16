using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using HammondsIBMS_2.Helpers;
using HammondsIBMS_2.Setup;
using HammondsIBMS_2.ViewModels.Customer;
using HammondsIBMS_Application;
using HammondsIBMS_Domain.Entities.Contracts;
using HammondsIBMS_Domain.Entities.Customers;
using Telerik.Web.Mvc;
using Telerik.Web.Mvc.Extensions;

namespace HammondsIBMS_2.Controllers
{
    public class CustomerController : FilterController<CustomerFilterVM>
    {
        private readonly CustomerService _customerService;
        private readonly DocumentController _documentController;
        private readonly EmployerService _employerService;


        public CustomerController(CustomerService svc, TransactionsService accountsService,DocumentController documentController,EmployerService employerService)
        {
            _customerService = svc;
            _documentController = documentController;
            _employerService = employerService;
            InitialisePaymentSourcesViewBag(accountsService);
            InitialiseEmployersViewBag(employerService);
        }

        #region View

        public ActionResult Index()
        {
            return IndexView();
        }

        private ActionResult IndexView()
        {
            return View("Index", _customerService.GetCustomers());
        }

        [GridAction]
        public ActionResult _CustomerGrdBind()
        {
            var mdl = _customerService.GetCustomers();

            if (FilterVM != null)
            {
                
                if (FilterVM.SearchString.HasValue())
                {
                    var srchStr = FilterVM.SearchString.ToUpper();
                    mdl = _customerService.GetCustomers().Where(m => m.CustomerId.ToString() == srchStr || 
                                                        (m.Surname.HasValue() && m.Surname.ToUpper().Contains(srchStr)) || 
                                                        (m.Surname.HasValue() && m.FirstName.ToUpper().Contains(srchStr)) || 
                                                        (m.Address.AddressLine1.HasValue() && m.Address.AddressLine1.ToUpper().Contains(srchStr)));
                }

            }
    
            var vm = AutoMapperSetup.MapList<Customer, CustomerListVM>(mdl);
            return View(new GridModel(vm));
        }

        #endregion

        #region Filtering

        protected override CustomerFilterVM ResetFilterVM()
        {
            return new CustomerFilterVM();
        }

        protected override void AddSelectors()
        {
            
        }

        #endregion

        #region Create New Customer Stepper

        public ActionResult CreateCustomer()
        {

            Stepper2.CreatePageStepper("Index","Add Customer");
            Stepper2.AddStep("CreateCustomerStep1");
            Stepper2.AddStep("CreateCustomerStep2");
            Stepper2.AddStep("CreateCustomerStep3");
            TempCustomer = new Customer{CustomerEmployer = new CustomerEmployer{EmployerId = -1}};
            return RedirectToAction(Stepper2.NextStep());
        }

        public ActionResult CreateCustomerStep1()
        {
            return View(TempCustomer);
        }

        [HttpPost]
        public ActionResult CreateCustomerStep1(Customer mCustomer)
        {
            if(ModelState.IsValid)
            {
                if (TryUpdateModel(TempCustomer))
                {
                    return RedirectToAction(Stepper2.NextStep());
                }
                
            }
            return View(mCustomer);
        }

     

        public ActionResult CreateCustomerStep2()
        {
            ViewBag.Employers = AddAllSelector<Employer>(new Employer {EmployerId = -1, EmployerName = " ---"},
                                                         ViewBag.Employers);
            return View(TempCustomer.CustomerEmployer);
        }

        [HttpPost]
        public ActionResult CreateCustomerStep2(CustomerEmployer mCustomerEmployer)
        {

            if(ModelState.IsValid)
            {
                if(TryUpdateModel(TempCustomer.CustomerEmployer))
                {
                    if (TempCustomer.CustomerEmployer.EmployerId == -1)
                    {
                        TempCustomer.CustomerEmployer = null;
                        TempCustomer.CustomerEmployerId = null;
                    }
                    return RedirectToAction(Stepper2.NextStep());
                }

            }
            ViewBag.Employers = AddAllSelector<Employer>(new Employer { EmployerId = -1, EmployerName = " ---" },
                                                        ViewBag.Employers);
            return View( mCustomerEmployer);
        }

        public ActionResult CreateCustomerStep3()
        {
            return View(Mapper.Map<Customer,CustomerBankPaymentDetailsVM>(TempCustomer));
        }

        [HttpPost]
        public ActionResult CreateCustomerStep3(CustomerBankPaymentDetailsVM mCustomer)
        {
            if(ModelState.IsValid)
            {
                var customer = TempCustomer;
                if(TryUpdateModel(customer))
                {
                    if (ExecuteRepositoryAction(() => { _customerService.AddCustomer(customer); _customerService.CommitChanges(); }))
                        return View("Edit", customer);
                }
            }
            return View(mCustomer);
        }

        public ActionResult _GetEmployerDetails(int id)
        {
            var emp = _employerService.GetEmployerById(id);
            return PartialView("_DisplayEmployerDetailsBody", emp);
        }

       
        public ActionResult _EmployersForAutoComplete(string text)
        {
            var employers = AddAllSelector<Employer>(new Employer { EmployerId = -1, EmployerName = " - No Employer Details -" },
                                                        ViewBag.Employers);
            return new JsonResult { Data = new SelectList(employers, "EmployerId", "EmployerName") };
        }

        private static Customer TempCustomer
        {
            get { return TempRepository<Customer>.Entity; }
            set { TempRepository<Customer>.Entity = value; }
        }

        #endregion

        #region create Customer
        public ActionResult CreateNewCustomer()
        {
            var newCust = new Customer();
            return PartialView("_EditCustomerDetails", newCust);
        }

        [HttpPost]
        public ActionResult CreateNewCustomer(Customer model)
        {
            if (TryValidateModel(model))
            {
                if (ExecuteRepositoryAction(() =>
                                                {
                                                    _customerService.AddCustomer(model);
                                                    _customerService.CommitChanges();
                                                }))
                    return Json(new {success = true, idx = model.CustomerId});
            }

            return PartialView("_EditCustomerDetails", model);
        }

        #endregion

        public ActionResult _Edit(int id)
        {
            var cust = _customerService.GetCustomer(id);
            return PartialView("Edit", cust);
        }

        public ActionResult Edit(int id,int moveOnToEditAccount=-1)
        {
            var cust = _customerService.GetCustomer(id);
            ViewBag.MoveOnToEditAccount = moveOnToEditAccount;
            return View(cust);
        }

        public ActionResult _DisplayCustomerEditHeader(int id)
        {
            var cust = _customerService.GetCustomer(id);
            return PartialView(cust);
        }

        [HttpPost]
        public ActionResult Edit(Customer mCustomer)
        {

            var etu = _customerService.GetCustomer(mCustomer.CustomerId);
            if (TryUpdateModel(etu))
            {
                if (ExecuteRepositoryAction(() =>
                                                {
                                                    _customerService.UpdateCustomer(etu);
                                                    _customerService.CommitChanges();
                                                }))
                    return RedirectToAction("Index", this.GridRouteValues());

            }
            return IndexView();
        }

        public ActionResult _DisplayDetails(int id)
        {
            var cust = _customerService.GetCustomer(id);
            return PartialView("_DisplayCustomerDetails", cust);
        }

        public ActionResult _EditCustomerDetails(int id)
        {
            var cust = _customerService.GetCustomer(id);
            return PartialView("_EditCustomerDetails", cust);
        }

        [HttpPost]
        public ActionResult _EditCustomerDetails(Customer mCust)
        {
            var etu = _customerService.GetCustomer(mCust.CustomerId);
            if (TryUpdateModel(etu))
            {
                if (ExecuteRepositoryAction(() =>
                                                {
                                                    _customerService.UpdateCustomer(etu);
                                                    _customerService.CommitChanges();
                                                }))
                    return Json(new {success = true});

            }
            return PartialView(mCust);
        }

        public ActionResult _EditPaymentDetails(int id)
        {
            var cust = _customerService.GetCustomer(id);
            return PartialView("_EditPaymentDetails", cust);
        }

        [HttpPost]
        public ActionResult _EditPaymentDetails(Customer mCust)
        {
            var cust = _customerService.GetCustomer(mCust.CustomerId);
            if (TryUpdateModel(cust))
            {
                if (ExecuteRepositoryAction(() =>
                    {
                        _customerService.UpdateCustomer(cust);
                        _customerService.CommitChanges();
                    }))
                    return ReturnJsonFormSuccess();
            }
            return PartialView(mCust);
        }

        public ActionResult _DisplayNameAddress(int id)
        {
            var cust = _customerService.GetCustomer(id);
            return PartialView("_DisplayNameAddress", cust);
        }

        public ActionResult _DisplayPaymentAndBankDetails(int id)
        {
            var cust = _customerService.GetCustomer(id);
            return PartialView(cust);
        }

        #region Customer Employer

        public ActionResult _DisplayEmployerDetails(int id)
        {
            var cust = _customerService.GetCustomer(id);
            return PartialView("_DisplayEmployerDetails", cust);
        }

        public ActionResult _EditCustomerEmployer(int id)
        {
            var emp = _customerService.GetCustomer(id).CustomerEmployer??new CustomerEmployer{EmployerId = -1};
            emp.CustomerId = id;
            return PartialView(emp);
        }

        [HttpPost]
        public ActionResult _EditCustomerEmployer(CustomerEmployer mEmployer)
        {
            var cus = _customerService.GetCustomer(mEmployer.CustomerId);
            if(cus.CustomerEmployer==null) cus.CustomerEmployer=new CustomerEmployer{EmployerId = -1};
            if (TryUpdateModel(cus.CustomerEmployer))
            {
                if (ExecuteRepositoryAction(() =>
                                                {
                                                    if(cus.CustomerEmployer.EmployerId==-1)
                                                        _customerService.DeleteCustomerEmployer(cus.CustomerEmployer);
                                                    else
                                                        _customerService.UpdateCustomer(cus);
                                                    _customerService.CommitChanges();
                                                }))
                    return Json(new {success = true});
            }

            return PartialView(mEmployer);
        }

        public ActionResult _DisplayEmployerDetailsBody(int id)
        {
            var emp = _customerService.GetEmployer(id);
            return PartialView(emp);
        }

        public ActionResult _DisplayCustomerEmployerDetailsBody(int id)
        {
            var cus = _customerService.GetCustomer(id);
            return PartialView(cus);
        }

        #endregion

        #region Documents

        public ActionResult _DisplayCustomerDocuments(int id)
        {
            ViewBag.CustomerId = id;
            return PartialView();
        }

        [GridAction]
        public ActionResult _GetCustomerDocuments(int id)
        {
            var docs = _customerService.GetCustomer(id).Documents;
            return View(new GridModel(AutoMapperSetup.MapList<CustomerDocument,CustomerDocumentVM>(docs)));
        }

        public ActionResult _AddDocument(int id)
        {
            var cd = new CustomerDocument {CustomerId = id};
            return PartialView("_EditCustomerDocument", Mapper.Map<CustomerDocument,CustomerDocumentVM>(cd));
        }

        [HttpPost]
        public ActionResult _AddDocument(CustomerDocumentVM mCustomerDocument)
        {
            var cd = new CustomerDocument();
            if(TryUpdateModel(cd))
            {
                cd.LoadFile(mCustomerDocument.FilePath);
                DocumentController.RemoveFile(mCustomerDocument.FilePath);
                if (ExecuteRepositoryAction(() =>
                                                {
                                                    _customerService.AddCustomerDocument(cd); _customerService.CommitChanges(); }))
                    return ReturnJsonFormSuccess();
            }

            return PartialView("_EditCustomerDocument", mCustomerDocument);
        }        

        public ActionResult _EditCustomerDocument(int id)
        {
            var cd = _customerService.GetCustomerDocument(id);
            return PartialView("_EditCustomerDocument", Mapper.Map<CustomerDocument, CustomerDocumentVM>(cd));
        }

        [HttpPost]
        public ActionResult _EditCustomerDocument(CustomerDocumentVM mCustomerDocument)
        {
            var cd = _customerService.GetCustomerDocument(mCustomerDocument.DocumentId);
            if (TryUpdateModel(cd))
            {
                cd.LoadFile(mCustomerDocument.FilePath);
                DocumentController.RemoveFile(mCustomerDocument.FilePath);
                if (ExecuteRepositoryAction(() => { _customerService.UpdateCustomerDocument(cd); _customerService.CommitChanges(); }))
                    return ReturnJsonFormSuccess();
            }
            return PartialView("_EditCustomerDocument", mCustomerDocument);
            
        }
        
        [HttpPost]
        [GridAction]
        public ActionResult _DeleteCustomerDocument(int id)
        {
            var cd = _customerService.GetCustomerDocument(id);
            var cusId = cd.CustomerId;
            ExecuteRepositoryAction(() =>
                                        {
                                            _customerService.DeleteCustomerDocument(cd);
                                            _customerService.CommitChanges();
                                        });
            return _GetCustomerDocuments(cusId);

        }

        public ActionResult _ViewCustomerDocument(int id)
        {
            return _documentController.ReturnDocumentForDisplayById(id);
        }

      #endregion

       
    }
}




