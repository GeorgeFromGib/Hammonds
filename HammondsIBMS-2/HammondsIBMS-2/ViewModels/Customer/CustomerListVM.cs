using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HammondsIBMS_2.ViewModels.Customer
{
    public class CustomerListVM
    {
         public string CustomerId{get;set;}
              public string Surname{get;set;}
              public string FirstName{get;set;}
              public string Tel{get;set;}
              public string Mobile{get;set;}
              public string AddressLine1{get;set;}
              public string AddressLine2{get;set;}
    }
}