using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HammondsIBMS_Domain.Entities.Contracts;

namespace HammondsIBMS_2.Helpers
{
    public static class TempRepository<T> where T:class
    {
        public static T Entity
        {
            get
            {
                var entity=HttpContext.Current.Session[SessionName] as T;
                return entity;
            }
            set { HttpContext.Current.Session[SessionName] = value; }
        }

        private static string SessionName
        {
            get { return "_tempEntity_" + GetEntityName(); }
        }

        private static string GetEntityName()
        {
            return typeof (T).ToString();
        }

    }

    public static class IntIndexIncrementer
    {
        
        public static int NextIndex<T>(this ICollection<T> collection,Func<T,int> indexId,int startFrom=0)
        {
            if (collection == null)
                return 0;
            if (collection.Count == 0)
                return 1 + startFrom;
            return collection.Max(indexId) + 1+startFrom;
        }
    }
}