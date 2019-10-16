using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;


// **** UNDER Development !!!!!!!!!!!!!!!
namespace HammondsIBMS_2.Helpers.Html
{
    public static class HtmlExstension2
    {
        public static GGBuilder GGBuilder(this HtmlHelper htmlHelper)
        {
            return new GGBuilder(htmlHelper);
        }
             
   
    }

    public class GGBuilder
    {
        private readonly HtmlHelper _htmlHelper;

        public GGBuilder(HtmlHelper htmlHelper )
        {
            _htmlHelper = htmlHelper;
        }

        public GGFormFilter<T> FormFilter<T>() where T:class
        {
            return new GGFormFilter<T>(_htmlHelper);
        }
    }

    public class GGFormFilter<TModel> where TModel:class
    {
        private readonly HtmlHelper _htmlHelper;
        private string _name;

        public GGFormFilter(HtmlHelper htmlHelper )
        {
            _htmlHelper = htmlHelper;
        }

        public GGFormFilter<TModel> Name(string name)
        {
            _name = name;
            return this;
        }

        public GGFormFilter<TModel> Rows(Action<FormFilterRowFactory<TModel>> configurator)
        {

            return this;
        }

        public MvcHtmlString Render()
        {
            return null;
        }

    }

    public class FormFilterRowFactory<TModel>
    {
        public FormFilterRow<TModel> Add()
        {
            return new FormFilterRow<TModel>();
        }
    }

    public class FormFilterRow<TModel>
    {

        public FormFilterRow<TModel> Field(Expression<Func<TModel,MvcHtmlString >> expression)
        {
            return null;
        }

        public FormFilterRow<TModel> Label(string name)
        {
            return null;
        }
     
    }
}