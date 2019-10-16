using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using FluentValidation.Mvc;
using HammondsIBMS_2.Controllers;
using HammondsIBMS_2.Helpers;
using HammondsIBMS_2.Setup;
using Telerik.Web.Mvc;

namespace HammondsIBMS_2
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            DataReferenceSetup.Setup();
            SiteMapSetup.Setup();
            UnityContainerSetup.SetUp();
            AutoMapperSetup.Setup();

            FluentValidationModelValidatorProvider.Configure();
        }

        protected void Session_Start(Object sender, EventArgs e)
        {
            Session[DocumentController.SessionName] = @"~\Upload\" + HttpContext.Current.Session.SessionID;
        }


        protected void Session_End(Object sender, EventArgs e)
        {
            ClearDocumentTempArea();
        }

        private void ClearDocumentTempArea()
        {
            var sessionDocumentTempArea = Session["_DocumentTempArea"] as string;
            if (Directory.Exists(sessionDocumentTempArea))
            {
                Directory.Delete(sessionDocumentTempArea, true);
            }
        }

       
    }

    public static class SiteMapSetup
    {
        public static void Setup()
        {
            SiteMapManager.SiteMaps.Register<XmlSiteMap>("Main", sitmap => sitmap.LoadFrom("~/Main.sitemap"));
        }
    }
}