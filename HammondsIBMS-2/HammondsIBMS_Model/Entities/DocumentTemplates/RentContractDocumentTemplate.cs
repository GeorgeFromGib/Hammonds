using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HammondsIBMS_Domain.Entities.Accounts;
using HammondsIBMS_Domain.Entities.Contracts;

namespace HammondsIBMS_Domain.Entities.DocumentTemplates
{
    public class RentContractDocumentTemplate : DocumentTemplate, IDocumentTemplateParserSource<RentalAccount>
    {
        public static Dictionary<string, Func<RentalAccount, string>> _availablefields = new Dictionary
            <string, Func<RentalAccount, string>>()
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
                {"RentedUnits",c=>ReturnRentedStock(c.RentedUnits)},
                {"RentalTotal",c=>c.TotalRental.ToString("C")},
                {"DepositTotal",c=>c.TotalDeposit.ToString("C")},
            };


        private static string ReturnRentedStock(List<RentedUnit> rentedUnits)
        {
            var str = new StringBuilder();
            str.Append("<table style='width:100%'>" +
                       "<tr style='background-color: whitesmoke'>" +
                       "<td >Model :</td>" +
                       "<td >Identifier :</td>" +
                       "<td >Date :</td>" +
                       "<td >Amount :</td>" +
                       "<td >Deposit :</td>" +
                       "<td >Notes :</td>" +
                       "</tr>");
            foreach (RentedUnit unit in rentedUnits)
            {
                str.AppendFormat("<tr>"+
                                 "<td>{0}</td>" +
                                 "<td>{1}</td>" +
                                 "<td>{2:d}</td>" +
                                 "<td>{3:C}</td>" +
                                 "<td>{4:C}</td>" +
                                 "<td>{5}</td>" +
                                 "</tr>",
                                 unit.Stock.ManufacturerModel,
                                 unit.Stock.Identifier,
                                 unit.RentedDate,
                                 unit.Amount,
                                 unit.Deposit,
                                 unit.Notes
                                 );
            }
            str.Append("</table>");
            return str.ToString();
        }
        
        
        public override IEnumerable<string> GetAllowedFieldslist()
        {
            return _availablefields.Select(i => i.Key).ToList();
        }


        public string GetFieldValue(string field, RentalAccount recordSource)
        {
            return _availablefields[StripDelimters(field)](recordSource);
        }
    }
}