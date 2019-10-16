using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using HammondsIBMS_Domain.Entities.Accounts;
using HammondsIBMS_Domain.Entities.Contracts;

namespace HammondsIBMS_Domain.Entities.DocumentTemplates
{
    public class PurchaseContractDocumentTemplate : DocumentTemplate, IDocumentTemplateParserSource<PurchaseAccount>
    {
        public static Dictionary<string, Func<PurchaseAccount, string>> _availablefields = new Dictionary
            <string, Func<PurchaseAccount, string>>()
            {
                {"Customer.CustomerId", c => c.CustomerId.ToString()},
                {"Customer.NameSurname", c => c.Customer.FirstName + " " + c.Customer.Surname},
                {"Customer.Address", c => DocumentTemplateEntityHelpers.ReturnAddess(c.Customer.Address)},
                {"Alternate.Address", c => DocumentTemplateEntityHelpers.ReturnAddess(c.AlternateAddress)},
                {"Customer.Tel1", c => c.Customer.ContactInfo.Tel},
                {"Customer.Tel2", c => c.Customer.ContactInfo.Tel2},
                {"Customer.Mobile1", c => c.Customer.ContactInfo.Mobile},
                {"Customer.Mobile2", c => c.Customer.ContactInfo.Mobile2},
                {"Customer.Email", c => c.Customer.ContactInfo.Email},
                {"Instructions", c => c.AlternateAddressInstructions},
                //{"ProductDetails",c=>ReturnProductDetails(c.Stock)},
                {"ContractDetails",c=>ReturnContractDetails(c.AttachedContracts)},
                {"OneOffItems",c=>ReturnOneOffItems(c.OneOffItems)},
            };


        public override IEnumerable<string> GetAllowedFieldslist()
        {
            return _availablefields.Select(i => i.Key).ToList();
        }


        public string GetFieldValue(string field, PurchaseAccount recordSource)
        {
            return _availablefields[StripDelimters(field)](recordSource);
        }


        public static string ReturnProductDetails(Stock.Stock stock)
        {
            return string.Format("<table style='width:100%'>" +
                                 "<tr style='background-color: whitesmoke'>" +
                                 "<td >HTV Identifier :</td>" +
                                 "<td >Model :</td>" +
                                 "<td >Serial :</td>" +
                                 "<td >Retail :</td>" +
                                 "<td >Discount:</td>" +
                                 "</tr>" +
                                 "<tr>" +
                                 "<td>{0}</td>" +
                                 "<td>{1}</td>" +
                                 "<td>{2}</td>" +
                                 "<td>{3:C}</td>" +
                                 "<td>{4:C}</td>" +
                                 "</tr>" +
                                 "</table>",
                                 stock.Identifier,
                                 stock.ManufacturerModel,
                                 stock.SerialNumber,
                                 stock.Model.RetailPrice,
                                 0
                                 );
        }

        public static string ReturnContractDetails(List<Contract> contacts)
        {
            var str = new StringBuilder();
            str.Append("<table style='width:100%'>" +
                       "<tr style='background-color: whitesmoke'>" +
                       "<td >Contract :</td>" +
                       "<td >Start :</td>" +
                       "<td >End :</td>" +
                       "<td >Charge :</td>" +
                       "<td >Period :</td>" +
                       "</tr>");
            foreach (ServiceContract contract in contacts)
            {
                str.AppendFormat("<tr><td>{0}</td><td>{1:d}</td><td>{2:d}</td><td>{3:C}</td><td>{4}</td></tr>",
                                 contract.ContractType.Description,
                                 contract.StartDate,
                                 contract.ExpiryDate,
                                 contract.Charge,
                                 contract.PaymentPeriod.Description);
            }
            str.Append("</table>");
            return str.ToString();
        }

        public static string ReturnOneOffItems(List<OneOffItem> oneOfItems)
        {
            var str = new StringBuilder();
            str.Append("<table style='width:100%'>" +
                       "<tr style='background-color: whitesmoke'>" +
                       "<td >Description :</td>" +
                       "<td >Charge :</td>" +
                       "</tr>");
            foreach (var oneOffItem in oneOfItems)
            {
                str.AppendFormat("<tr><td>{0}</td><td>{1:c}</td>",
                                 oneOffItem.Description,
                                 oneOffItem.Charge
                                 );
            }
            str.Append("</table>");
            return str.ToString();
        }
     
    }
}