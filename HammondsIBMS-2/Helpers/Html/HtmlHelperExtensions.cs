using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using System.Web.Routing;
using Telerik.Web.Mvc.UI.Fluent;
using Telerik.Web.Mvc.UI;

namespace HammondsIBMS_2.Helpers.Html
{
   
    public class ActionWindowParameters:WindowParameters
    {
        

        public string LinkText { get; set; }

        public ActionWindowParameters(string name,string headerText,string action, string controller,string linkText,int widthInPx=300,int heightInPx=200,object routeValues=null):base(name,headerText,action,controller,widthInPx,heightInPx,routeValues)
        {
            LinkText = linkText;
        }

    }

    public class WindowParameters
    {
        public string Name { get; set; }
        public string HeaderText { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public int WidthInPx { get; set; }
        public int HeightInPx { get; set; }
        public object RouteValues { get; set; }

        public WindowParameters(string name, string headerText, string action, string controller,int widthInPx = 300, int heightInPx = 200,object routeValues=null)
        {
            Name = name;
            Action = action;
            Controller = controller;
            HeaderText = headerText;
            WidthInPx = widthInPx;
            HeightInPx = heightInPx;
            RouteValues = routeValues;
        }
    }

    public class ActionButtonParameters
    {
        public string Name { get; set; }
        public object RouteValues { get; set; }
        public ButtonIcon ButtonIcon { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string UrlAction { get; set; }

        public ActionButtonParameters(string name, string controller,string action, object routeValues, ButtonIcon buttonIcon = ButtonIcon.Edit)
        {
            Name = name;
            Controller = controller;
            Action = action;
            RouteValues = routeValues;
            ButtonIcon = buttonIcon;
        }

        public ActionButtonParameters(string name, string urlAction, ButtonIcon buttonIcon = ButtonIcon.Edit)
        {
            Name = name;
            UrlAction = urlAction;
            ButtonIcon = buttonIcon;
        }


    }

  

    public class ActionParameters
    {
        public string Controller { get; set; }
        public string Action { get; set; }
        public object RouteValues { get; set; }

        public ActionParameters(string action):this(action,null,null)
        {
        }

        public ActionParameters(string action,string controller=null,object routeValues=null )
        {
            Action = action;
            Controller = controller;
            RouteValues = routeValues;
        }

        public ActionParameters(string action,object routeValues=null):this(action,null,routeValues)
        {      
        }
    }

    public class ActionButtonWindowParameters
    {
        public string Name { get; set; }
        public string HeaderText { get; set; }
        public int WidthInPx { get; set; }
        public int HeightInPx { get; set; }
        public ActionWindowTitleBarButtons TitleBarButtons { get; set; }
        public string OnWindowCloseClientFunction { get; set; }

        public ActionButtonWindowParameters(string name, string headerText, int widthInPx = 300, int heightInPx = 200, ActionWindowTitleBarButtons titleBarButtons = ActionWindowTitleBarButtons.Close,string onWindowCloseClientFunction=null)
        {
            Name = name;
            HeaderText = headerText;
            WidthInPx = widthInPx;
            HeightInPx = heightInPx;
            TitleBarButtons = titleBarButtons;
            OnWindowCloseClientFunction = onWindowCloseClientFunction;
        }
    }

    [Flags]
    public enum ActionWindowTitleBarButtons
    {
        None=1,
        Close=2,
        Maximise=4,
        Refresh=8
    }

    public class ActionDialogFormParamters
    {
        public string UpdateTargetId { get; set; }
        public string UpdateUrl { get; set; }
        public string DialogTitle { get; set; }
        public string UpdateUrlFormIndexFieldName { get; set; }
        public string OnSuccessClientFunction { get; set; }
        public string Action { get; set; }


        public ActionDialogFormParamters(string dialogTitle, string updateTargetId, string updateUrl, string updateUrlFormIndexFieldName = null, string onSuccessClientFunction=null)
        {
            DialogTitle = dialogTitle;
            UpdateTargetId = updateTargetId;
            UpdateUrl = updateUrl;
            UpdateUrlFormIndexFieldName = updateUrlFormIndexFieldName;
            OnSuccessClientFunction = onSuccessClientFunction;

        }

        public ActionDialogFormParamters(string dialogTitle, string action, string updateUrlFormIndexFieldName = null)
        {
            DialogTitle = dialogTitle;
            Action = action;
            UpdateUrlFormIndexFieldName = updateUrlFormIndexFieldName;
        }

        
    }

    public class GridButtonActionParameters : ActionButtonParameters
    {
        public GridButtonType GridButtonType { get; set; }

        public GridButtonActionParameters(string name, string controller,string action, object routeValues, ButtonIcon buttonIcon = ButtonIcon.Edit, GridButtonType gridButtonType = GridButtonType.Image):base(name,controller,action,routeValues,buttonIcon)
        {
            GridButtonType = gridButtonType;
        }

        public GridButtonActionParameters(string name,string urlAction , ButtonIcon buttonIcon = ButtonIcon.Edit, GridButtonType gridButtonType = GridButtonType.Image):base(name,urlAction,buttonIcon)
        {
            GridButtonType = gridButtonType;
        }
    }

    public class GridButtonWindowActionParameters : GridButtonActionParameters
    {
        public string WindowName { get; set; }


        public GridButtonWindowActionParameters(string name, string windowName, string controller, string action, object routeValues, ButtonIcon buttonIcon = ButtonIcon.Edit, GridButtonType gridButtonType = GridButtonType.Image)
            : base(name, controller, action, routeValues, buttonIcon, gridButtonType)
        {
            WindowName = windowName;
        }
    }

   

    public static class HtmlHelperExtensions
    {
        public static HtmlString ContentTitle(this HtmlHelper helper, string title)
        {

                
           return new HtmlString("<div class='well well-small'><h5>" + title + "</h5></div>");
        }

        public static MvcHtmlString Separator(this HtmlHelper htmlHelper,string title )
        {
            //var htmlStr = string.Format("<div class='separator'><p class='t-reset' ><span class='t-icon t-collapse' /><strong>{0}</strong></p></div>", title);
            var htmlStr = String.Format("<span class='label label-info'>{0}</span>",title);
            return new MvcHtmlString(htmlStr);
        }

        public static MvcAnchor Container(this HtmlHelper htmlHelper, string title=null)
        {
            htmlHelper.ViewContext.Writer.Write(String.Format("<fieldset>{0}",title!=null?String.Format("<legend>{0}</legend>",title):""));
            return new MvcAnchor(htmlHelper.ViewContext, "</fieldset>");
        }

        public static MvcHtmlString ProgressBar(this HtmlHelper htmlHelper, string name, int initialValue = 0)
        {
            return ProgressBar(htmlHelper, name, ButtonRole.Default, initialValue);
        }

        public static MvcHtmlString ProgressBar(this HtmlHelper htmlHelper, string name, ButtonRole buttonRole, int initialValue = 0)
        {
            return ProgressBarConstructor.ConstructBar(name, buttonRole, initialValue);
        }

        //Needs a View Named _Filter and _FilterReset
        public static MvcHtmlString FormFilter(this HtmlHelper htmlHelper, string onChangeFunction)
        {
            var ctrl = "~/"+htmlHelper.ViewContext.RouteData.Values["Controller"];
            var filterAction = ctrl+"/_Filter";
            var resetAction = ctrl + "/_FilterReset";
            return FormFilter(htmlHelper, UrlHelper.GenerateContentUrl(filterAction,htmlHelper.ViewContext.HttpContext), onChangeFunction, true, UrlHelper.GenerateContentUrl(resetAction,htmlHelper.ViewContext.HttpContext));
        }

        public static MvcHtmlString FormFilter(this HtmlHelper htmlHelper, string urlAction, string onChangeFunction, string resetUrlAction)
        {
            return FormFilter(htmlHelper, urlAction, onChangeFunction, true, resetUrlAction);
        }

        public static MvcHtmlString FormFilter(this HtmlHelper htmlHelper, string urlAction,string onChangeFunction,bool includeReset,string resetUrlAction)
        {
            var fromTag = new TagBuilder("Form");
            var id = "_filter_" + Guid.NewGuid().ToString();
            fromTag.Attributes.Add("action",urlAction);
            fromTag.AddCssClass("form-inline form-filter form-search");
            fromTag.Attributes.Add("method","post");
            fromTag.Attributes.Add("data-onchangefunction",onChangeFunction);
            fromTag.InnerHtml = String.Format("<table><tr><td id={0}>{1}</td><td style='width: 100%'><div class='btn-group'>{2} {3}</div></td></tr></table>",
                                              id,
                                              htmlHelper.Action(urlAction.Substring(urlAction.LastIndexOf('/')+1)).ToHtmlString(),
                                              htmlHelper.FormSubmitButton(new ButtonParameters(ButtonIcon.Filter, "Filter")).ToHtmlString(),
                                              (includeReset?htmlHelper.ClientButton(new ButtonParameters(ButtonIcon.FilterClear,"Clear"),null,null,"filter-reset",new {data_resetAction=resetUrlAction,data_onChangefunction=onChangeFunction,data_idToUpdate=id}).ToHtmlString():""));
            return new MvcHtmlString(fromTag.ToString());
        }

       

        public static MvcHtmlString CheckBoxLabelled(this HtmlHelper helper,string name,bool isChecked,string text)
        {
            var htmlStr = new StringBuilder("<label class='checkbox'>");
            htmlStr.Append(helper.CheckBox(name, isChecked, new {id=name}).ToHtmlString());
            htmlStr.Append(text+"</label>");
            return new MvcHtmlString(htmlStr.ToString());
        }

        public static MvcHtmlString ActionWindow(this HtmlHelper helper,ActionWindowParameters parameters)
        {
            var str = new StringBuilder();
            str.Append(String.Format("<a id='{0}'class='t-button actionWindow'  href='#'>{1}</a>",parameters.Name, parameters.LinkText));
            
            var wdw = WindowBuilderHelper.CreateWindow(helper, parameters);

            str.Append(wdw.ToHtmlString());

            return new MvcHtmlString(str.ToString());

        }


        public static MvcHtmlString GridButtonActionWindow(this HtmlHelper helper, string name, string headerText,int widthInPx = 300, int heightInPx = 200)
        {
            var wp = new WindowParameters(name, headerText, "", "", widthInPx, heightInPx, "");
            var wdw = WindowBuilderHelper.CreateWindow(helper, wp);
            return new MvcHtmlString(wdw.ToHtmlString());
        }

        public static MvcHtmlString GridButtonAction(this HtmlHelper helper,GridButtonWindowActionParameters parameters)
        { 
            var idToInsert = parameters.Name;
            const string classToInsert = "gridButtonWindow";
            var winName = parameters.WindowName.ToUpper().EndsWith("WINDOW") ? parameters.WindowName : parameters.WindowName + "Window";
            var attrToInsert = String.Format("data-window='{0}' data-action='{1}'",
                                            winName, UrlHelper.GenerateContentUrl("~/" + parameters.Controller + "/" + parameters.Action+ "?" +
                                                              SerializeToQueryString(parameters.RouteValues),
                                                              helper.ViewContext.HttpContext));

            var str=GridButtonHTML.GetHTMLForButtonType(parameters.GridButtonType,parameters.ButtonIcon);
           
            str = HTMLHelper.AddIdToHtmlElement(str, idToInsert);
            str = HTMLHelper.AddClassToHtmlElement(str, classToInsert);
            str = HTMLHelper.AddAttributesToHtmlElement(str, attrToInsert);
            
            return new MvcHtmlString(str);
        }

        public static MvcHtmlString GridButtonAction(this HtmlHelper helper,GridButtonActionParameters parameters)
        {
            var idToInsert = parameters.Name;
            var classToInsert = "gridButtonToController";
            var actionComplete = "";
            if (String.IsNullOrEmpty(parameters.UrlAction))
                actionComplete = UrlHelper.GenerateContentUrl("~/" + parameters.Controller + "/" + parameters.Action+ "?" +
                                                              SerializeToQueryString(parameters.RouteValues),
                                                              helper.ViewContext.HttpContext);
            else
                actionComplete = parameters.UrlAction;
 
            var str = GridButtonHTML.GetHTMLForButtonType(parameters.GridButtonType, parameters.ButtonIcon,actionComplete);

            str = HTMLHelper.AddIdToHtmlElement(str, idToInsert);
            str = HTMLHelper.AddClassToHtmlElement(str, classToInsert);

            return new MvcHtmlString(str); 
        }

        //public static MvcHtmlString LabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes)
        //{
        //    return LabelFor(html, expression, new RouteValueDictionary(htmlAttributes));
        //}

        //public static MvcHtmlString LabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, IDictionary<string, object> htmlAttributes)
        //{
        //    ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
        //    string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
        //    string labelText = metadata.DisplayName ?? metadata.PropertyName ?? htmlFieldName.Split('.').Last();
        //    if (String.IsNullOrEmpty(labelText))
        //    {
        //        return MvcHtmlString.Empty;
        //    }
        //    TagBuilder tag = new TagBuilder("label");
        //    tag.MergeAttributes(htmlAttributes);
        //    tag.Attributes.Add("for", html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName));
        //    tag.SetInnerText(labelText);
        //    return MvcHtmlString.Create(tag.ToString(TagRenderMode.Normal));
        //}


        //private static WindowBuilder CreateDialogFormWindow(HtmlHelper helper, ActionButtonWindowParameters actionButtonWindowParameters, ActionParameters actionParameters)
        //{

        //}

        public static string SerializeToQueryString(object source)
        {
            if (source == null)
                return "";

            return String.Join(
                "&",
                source.GetType().GetProperties()
                    .Select(p => new { Name = p.Name, Value = p.GetValue(source, new object[] { }) })
                    .Where(p => p.Value != null)
                    .Select(p => String.Format("{0}={1}", p.Name, p.Value))
                    .ToArray()
            );
        }


        public static MvcHtmlString GridTopButtonActionWindow(this HtmlHelper helper,ActionWindowParameters parameters)
        {
            var str = new StringBuilder();

            str.Append(String.Format("<div class='t-toolbar t-grid-toolbar t-grid-top'>" +
                                     "<a id='{0}' class='t-button t-button-icontext t-grid-add actionWindow' " +
                                     "href='#'><span class='t-icon t-add'></span>{1}</a></div>", parameters.Name, parameters.LinkText));
            var wdw = WindowBuilderHelper.CreateWindow(helper, parameters);
            str.Append(wdw.ToHtmlString());
            return new MvcHtmlString(str.ToString());
        }

        public static MvcHtmlString GridTopDialogForm(this HtmlHelper helper, ButtonParameters buttonParameters, ActionParameters actionParameters, ActionDialogFormParamters actionDialogFormParamters)
        {
            var fdButton = helper.ActionDialogFormButton(buttonParameters, actionParameters, actionDialogFormParamters).ToString();
            var gridTopDiv = "<div class='t-toolbar t-grid-toolbar t-grid-top'>DIAGFORMBUTTON</div>";
            var comb = gridTopDiv.Replace("DIAGFORMBUTTON", fdButton);
            return new MvcHtmlString(comb);
        }

        public static MvcHtmlString EditorForWithEvents<TModel, TValue>(this HtmlHelper<TModel> helper,Expression<Func<TModel, TValue>> expression,string onChange)
        {
            var edt = helper.EditorFor(expression,new {@class="notifyOnChange"});
            return new MvcHtmlString(edt.ToHtmlString());
        }

        public static string CurrentVersion(this HtmlHelper helper)
        {
            try
            {
                var version = Assembly.GetExecutingAssembly().GetName().Version;
                return version.ToString();
            }
            catch
            {
                return "?.?.?.?";
            }
        }
    }


    public static class AjaxHelperExtensions
    {
      

        [Obsolete("Use html.FormSubmitButton")]
        public static MvcHtmlString SubmitButton(this AjaxHelper ajaxHelper,ButtonType buttonType=ButtonType.Image,ButtonIcon buttonIcon=ButtonIcon.Ok)
        {
            return SubmitButton(ajaxHelper, buttonType, "", buttonIcon);
        }

        [Obsolete("Use html.FormSubmitButton")]
        public static MvcHtmlString SubmitButton(this AjaxHelper ajaxHelper,ButtonType buttonType,string buttonText,ButtonIcon buttonIcon=ButtonIcon.Ok)
        {
            var htmlStr = "<button  type='submit'>REPLACE</button>";
            var button = new ButtonParameters(buttonIcon, buttonType, buttonText);
            return button.ButtonConstructor(htmlStr);
        }

    }


    //internal static class ActionButtonHTML
    //{
    //    private static Dictionary<ButtonType, string> _htmlButtonType = new Dictionary<ButtonType, string>()
    //                                                                            {
    //                                                                                {ButtonType.Image,"t-button t-button-icon"},
    //                                                                                {ButtonType.ImageText,"t-button t-button-icontext"}
                                                                    
    //                                                                            };

    //    private static Dictionary<ButtonIcon, string> _htmlButtonAction = new Dictionary<ButtonIcon, string>()
    //                                                                            {
    //                                                                                {ButtonIcon.Add,"<span class='t-icon t-add'></span>INNERTEXT"},
    //                                                                                {ButtonIcon.Delete,"<span class='t-icon t-delete'></span>INNERTEXT"},
    //                                                                                {ButtonIcon.Ok,"<span class='t-icon t-update'></span>INNERTEXT"},
    //                                                                                {ButtonIcon.Cancel,"<span class='t-icon t-cancel'></span>INNERTEXT"},
    //                                                                                {ButtonIcon.Refresh,"<span class='t-icon t-refresh'></span>INNERTEXT"},
    //                                                                                {ButtonIcon.Edit,"<span class='t-icon t-edit'></span>INNERTEXT"},
    //                                                                                {ButtonIcon.Filter,"<span class='t-icon t-filter'></span>INNERTEXT"},
    //                                                                                {ButtonIcon.FilterClear,"<span class='t-icon t-clear-filter'></span>INNERTEXT"},
    //                                                                                {ButtonIcon.Search,"<span class='t-icon t-search'></span>INNERTEXT"},
    //                                                                                {ButtonIcon.List, PrepareHtmlForIcon("list_16x16.png","List","middle")},
    //                                                                                {ButtonIcon.Return,PrepareHtmlForIcon("return_16x16.png","Return","top")},
    //                                                                                {ButtonIcon.Work,PrepareHtmlForIcon("work_16x16.png","Work","top")},
    //                                                                                {ButtonIcon.Camera,PrepareHtmlForIcon("camera_16x16.png","Camera","middle")},
    //                                                                                {ButtonIcon.Warning,PrepareHtmlForIcon("warning_16x16.png","Warning","middle")},
    //                                                                                {ButtonIcon.Document,PrepareHtmlForIcon("document_16x16.png","Document","middle")},
    //                                                                                {ButtonIcon.Exchange,PrepareHtmlForIcon("exchange_16x16.png","Document","middle")}
                                                                    
    //                                                                            };

    //    private static string PrepareHtmlImgSrc(string imagePath,string alt,string align)
    //    {
    //        var htmlStr=string.Format(@"<img src='{0}' border='0' align='{1}' alt='{2}' /> INNERTEXT",VirtualPathUtility.ToAbsolute(imagePath),align,alt);
    //        return htmlStr;
    //    }

    //    private static string PrepareHtmlForIcon(string iconName,string alt,string align)
    //    {
    //        return PrepareHtmlImgSrc(string.Format(@"~/Content/img/icons/{0}", iconName), alt, align);
    //    }

    //    public  static string GetClassForButtonType(ButtonType buttonType)
    //    {
    //        return _htmlButtonType[buttonType];
    //    }

    //    public static string GetInnerHtmlForbuttonIcon(ButtonIcon buttonIcon,string buttonText="")
    //    {
    //        var html=_htmlButtonAction[buttonIcon].Replace("INNERTEXT",buttonText);
    //        return html;
    //    }

    //    //public static MvcHtmlString ActionButtonConstructor(ButtonType buttonType, string buttonText,
    //    //                                                      ButtonIcon buttonIcon, string htmlStr)
    //    //{
    //    //    return ActionButtonConstructor(new ButtonParameters(buttonIcon, buttonType, buttonText), htmlStr);
    //    //}

    //    public static MvcHtmlString ActionButtonConstructor(ButtonParameters buttonParameters,string htmlStr)
    //    {
    //        htmlStr = HTMLHelper.AddClassToHtmlElement(htmlStr, GetClassForButtonType(buttonParameters.Type));
    //        htmlStr = (!buttonParameters.Enabled) ? HTMLHelper.AddAttributesToHtmlElement(htmlStr,"disabled='disabled'") : htmlStr;
    //        var innerHtml = GetInnerHtmlForbuttonIcon(buttonParameters.Icon, buttonParameters.Text);
    //        innerHtml = (!buttonParameters.Enabled) ? HTMLHelper.AddAttributesToHtmlElement(innerHtml, "disabled='disabled'") : innerHtml;
    //        htmlStr = (buttonParameters.Width != null) ? HTMLHelper.AddStyleAttributeToHtmlElement(htmlStr, string.Format("width:{0};text-align:left", buttonParameters.Width)) : htmlStr;
    //        htmlStr = htmlStr.Replace("REPLACE", innerHtml);
    //        return new MvcHtmlString(htmlStr);
    //    }
    //}

    internal static class GridButtonHTML
    {
       
        private static Dictionary<GridButtonType, string > _htmlButonType = new Dictionary<GridButtonType, string>(){
                                                                         {GridButtonType.Image, "t-button t-button-icon {CLASS}"}

                                                                     };

        private static Dictionary<ButtonIcon, TypeStrings> _htmlButtonAction = new Dictionary
            <ButtonIcon, TypeStrings>()
                                                                                    {
                                                                                        {ButtonIcon.Edit,new TypeStrings("t-grid-edit","<span class='t-icon t-edit'></span>")},
                                                                                        {ButtonIcon.Delete,new TypeStrings("t-grid-delete","<span class='t-icon t-delete'></span>")},
                                                                                        {ButtonIcon.List,new TypeStrings("","<img src='"+VirtualPathUtility.ToAbsolute("~/Content/img/icons/list_16x16.png")+"' border='0' align='middle' alt='List' />")},
                                                                                         {ButtonIcon.Camera,new TypeStrings("","<img src='"+VirtualPathUtility.ToAbsolute("~/Content/img/icons/Camera_16x16.png")+"' border='0' align='middle' alt='List' />")}

                                                                                    };

        public static string GetHTMLForButtonType(GridButtonType gridButtonType, ButtonIcon buttonIcon,string actionComplete="#")
        {
            var htmlStr =
                string.Format(
                    "<a class='{0}'  href='{2}'>{1}</a>",
                    GetClasses(gridButtonType,buttonIcon),
                    GetInner(buttonIcon),
                    actionComplete);
            return htmlStr;

        }

        private static string GetClasses(GridButtonType gridButtonType,ButtonIcon buttonIcon)
        {
            var ms=_htmlButonType[gridButtonType].Replace("{CLASS}",_htmlButtonAction[buttonIcon].CssClasses);
            return ms;
        }

        private static string GetInner(ButtonIcon buttonIcon)
        {
            return _htmlButtonAction[buttonIcon].Inner;
        }

        private class TypeStrings
        {
            public string CssClasses { get; set; }
            public string Inner { get; set; }

            public TypeStrings(string cssClass, string inner)
            {
                CssClasses = cssClass;
                Inner = inner;
            }
        }
       
    }

    internal static class HTMLHelper
    {
        public static string AddIdToHtmlElement(string htmlElement, string idToInsert)
        {
            return htmlElement.Insert(htmlElement.IndexOf(">"), " id='" + idToInsert + "' ");
        }

        public static string AddAttributesToHtmlElement(string htmlElement, string attrToInsert)
        {
            return htmlElement.Insert(htmlElement.IndexOf(">"), " " + attrToInsert + " ");
        }

        public static string AddAttributesToHtmlElement(string htmlElement, object htmlAttributes)
        {
            if (htmlAttributes == null)
                return htmlElement;

            var attribs = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            var strToInsert = "";
            foreach (KeyValuePair<string, object> attrib in attribs)
            {
                strToInsert = strToInsert + (attrib.Key + "='" + attrib.Value + "'");
                htmlElement=AddAttributesToHtmlElement(htmlElement, strToInsert);
            }
            return htmlElement;
        }

        public static string AddStyleAttributeToHtmlElement(string htmlElement, string styleToInsert)
        {
            var htmlEl = htmlElement.Replace("\"", "'");

            if (htmlEl.IndexOf("style=") == -1)
            {
                htmlEl = AddAttributesToHtmlElement(htmlEl, "style=''");
            }
            var sti = styleToInsert.Trim();
            if (sti.LastIndexOf(";") == sti.Length)
                sti = sti.Remove(sti.LastIndexOf(";"));
            return htmlEl.Insert(htmlEl.IndexOf("style='") + 7, sti + "; ");
        }

        public static string AddClassToHtmlElement(string htmlElement, string classToInsert)
        {
            // Guard against \" in html

           var htmlEl = htmlElement.Replace("\"","'");

            if(htmlEl.IndexOf("class=")==-1)
            {
                htmlEl = AddAttributesToHtmlElement(htmlEl, "class=''");
            }
            return htmlEl.Insert(htmlEl.IndexOf("class='") + 7, classToInsert + " ");

        }
    }

    public class MvcAnchor : IDisposable
    {
        private readonly string _closingTag;
        private readonly TextWriter _writer;

        public MvcAnchor(ViewContext viewContext,string closingTag)
        {
            _closingTag = closingTag;
            _writer = viewContext.Writer;
        }

        public void Dispose()
        {
            this._writer.Write(_closingTag);
        }
    }

}