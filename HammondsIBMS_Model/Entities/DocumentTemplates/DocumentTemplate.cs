using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using FluentValidation;
using FluentValidation.Attributes;
using FluentValidation.Results;
using FluentValidation.Validators;
using HammondsIBMS_Domain.Entities.Accounts;


namespace HammondsIBMS_Domain.Entities.DocumentTemplates
{

    public abstract class DocumentTemplate
    {
        
        public int DocumentTemplateId { get;  set; }
        public string Title { get; set; }
        public string Body { get; set; }

        public static string FieldDelimter = "$";


        public IEnumerable<string> GetAllowedFieldsWithDelimeters()
        {
            return GetAllowedFieldslist().Select(DelimetedField);
        }

        protected static string DelimetedField(string field)
        {
            return string.Format("{0}{1}{0}",FieldDelimter,field);
        }

        public abstract IEnumerable<string> GetAllowedFieldslist();

        protected string StripDelimters(string field)
        {
            return field.Replace(FieldDelimter, "");
        }

    }

    public enum DocumentTemplateTypes
    {
        Sale=1,
        Rent=2
    }

   
 
}
