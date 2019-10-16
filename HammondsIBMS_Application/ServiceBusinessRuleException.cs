using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace HammondsIBMS_Application
{
   
    public class BusinessRuleException:Exception
    {
        public string FieldName { get; private set; }

        public BusinessRuleException(string fieldName, string ex) : base(ex)
        {
            FieldName = fieldName;
        }


        public static string PropertyName<E>(Expression<Func<E, object>> fieldExp)
        {
            var membExpr = fieldExp as MemberExpression;
            return membExpr == null ? "" : membExpr.Member.Name;
        }
    }


}
