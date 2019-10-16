using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HammondsIBMS_2.ViewModels.BaseVM;
using HammondsIBMS_2.ViewModels.ModelView;

namespace HammondsIBMS_2.Helpers
{
    // For a future development
    public class FilterHelper<T> where T : FilterBaseVM
    {
        private readonly Controller _controller;
        private readonly string _filterName;

        public FilterHelper(Controller controller,string filterName)
        {
            _controller = controller;
            _filterName = filterName;
        }

        private T ModelFilter
        {
            get
            {
                return _controller.HttpContext.Session["_"+_filterName] as T;
            }
            set { _controller.HttpContext.Session["_"+_filterName] = value; }
        }



    }
}