using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using HammondsIBMS_2.Helpers.Html;

namespace HammondsIBMS_2.Helpers.Html
{
    public static class HtmlButtonExtender
    {
        public static MvcHtmlString LinkButton(this HtmlHelper helper, string text, string onClickFunction = "", string link = "#")
        {
            return new MvcHtmlString(string.Format("<a class='t-button' onclick='{0}' href='{1}'>{2}</a>", onClickFunction, link, text));
        }

        public static MvcHtmlString FormSubmitButton(this HtmlHelper htmlHelper,string id="__FormSubmit")
        {
            return FormSubmitButton(htmlHelper, new ButtonParameters(ButtonIcon.Ok,ButtonType.Image,null,ButtonRole.Primary,ButtonIconColor.White,ButtonHeight.Small,true,null),id);
        }

        public static MvcHtmlString FormSubmitButton(this HtmlHelper htmlHelper, ButtonParametersBase buttonParameters,string id=null)
        {
            var htmlStr = string.Format("<button id='{0}' type='submit'>REPLACE</button>",id);
            return buttonParameters.ButtonConstructor(htmlStr);
        }



        //public static MvcHtmlString ActionButton(this HtmlHelper htmlHelper, ButtonType buttonType, string buttonText, ButtonIcon buttonIcon, string actionName)
        //{
        //    var htmlStr = htmlHelper.ActionLink("REPLACE", actionName).ToHtmlString();
        //    return ActionButtonHTML.ActionButtonConstructor(buttonType, buttonText, buttonIcon, htmlStr);
        //}


        //public static MvcHtmlString ActionButton(this HtmlHelper htmlHelper, ButtonType buttonType, ButtonIcon buttonIcon, string actionName)
        //{
        //    return ActionButton(htmlHelper, buttonType, "", buttonIcon, actionName);
        //}

        public static MvcHtmlString ActionButton(this HtmlHelper htmlHelper, ButtonParametersBase buttonParameters, string action,object htmlAttributes=null)
        {
            var htmlStr = string.Format("<a href='{0}'>REPLACE</a>", action);
            htmlStr = HTMLHelper.AddAttributesToHtmlElement(htmlStr, htmlAttributes);
            return buttonParameters.ButtonConstructor(htmlStr);
        }

        public static MvcHtmlString ActionButton(this AjaxHelper ajaxHelper, ButtonParameters buttonParameters, string actionName, AjaxOptions ajaxOptions)
        {
            return ActionButton(ajaxHelper, buttonParameters, new ActionParameters(actionName), ajaxOptions);
        }

        public static MvcHtmlString ActionButton(this AjaxHelper ajaxHelper, ButtonParameters buttonParameters, ActionParameters actionParameters, AjaxOptions ajaxOptions)
        {
            var htmlStr = ajaxHelper.ActionLink("REPLACE",actionParameters.Action,actionParameters.Controller,actionParameters.RouteValues,ajaxOptions).ToHtmlString();
            return buttonParameters.ButtonConstructor(htmlStr);
        }

       


        public static MvcHtmlString ActionWindowButton(this HtmlHelper htmlHelper, ButtonParametersBase buttonParameters, ActionParameters actionParameters, ActionButtonWindowParameters actionButtonWindowParameters)
        {
            var str = string.Format("<a id='{0}' href='#' class='actionWindow'>REPLACE</a>", actionButtonWindowParameters.Name);
            str = buttonParameters.ButtonConstructor(str).ToString();
            str += WindowBuilderHelper.CreateWindow(htmlHelper, actionButtonWindowParameters, actionParameters);
            return new MvcHtmlString(str);
        }

        public static MvcHtmlString ActionDialogFormButton(this HtmlHelper htmlHelper, ButtonParametersBase buttonParameters, ActionParameters actionParameters, ActionDialogFormParamters actionDialogFormParamters)
        {
            var tBuilder = new TagBuilder("a");
            tBuilder.SetInnerText("REPLACE");
            if (actionParameters.Action.Substring(0, 1) == "/")
                tBuilder.Attributes.Add("href", actionParameters.Action);
            else
                tBuilder.Attributes.Add("href", new UrlHelper(htmlHelper.ViewContext.RequestContext).Action(actionParameters.Action, actionParameters.Controller, actionParameters.RouteValues));

            tBuilder.Attributes.Add("data-dialog-title", actionDialogFormParamters.DialogTitle);
            tBuilder.Attributes.Add("data-update-target-id", actionDialogFormParamters.UpdateTargetId);
            tBuilder.Attributes.Add("data-update-url", actionDialogFormParamters.UpdateUrl);
            tBuilder.Attributes.Add("data-update-url-index", actionDialogFormParamters.UpdateUrlFormIndexFieldName);
            tBuilder.Attributes.Add("data-onSuccessfunction", actionDialogFormParamters.OnSuccessClientFunction);
            tBuilder.Attributes.Add("data-action", actionDialogFormParamters.Action);

            tBuilder.AddCssClass("dialogLink");

            var str = buttonParameters.ButtonConstructor(tBuilder.ToString()).ToString();

            return new MvcHtmlString(str);
        }

        public static MvcHtmlString ActionDialogPromptButton(this HtmlHelper htmlHelper,
                                                             ButtonParametersBase buttonParameters, string action,string promptTitle,string onSuccessClientFunction=null)
        {
            return ActionDialogFormButton(htmlHelper, buttonParameters, new ActionParameters(action),
                                          new ActionDialogFormParamters(promptTitle, null,action,null,onSuccessClientFunction));
        }


        public static MvcHtmlString ActionDialogLink(this HtmlHelper htmlHelper,string linkText, ActionParameters actionParameters, ActionDialogFormParamters actionDialogFormParamters)
        {
            var tBuilder = new TagBuilder("a");
            tBuilder.SetInnerText(linkText);
            tBuilder.Attributes.Add("href", new UrlHelper(htmlHelper.ViewContext.RequestContext).Action(actionParameters.Action, actionParameters.Controller, actionParameters.RouteValues));
            tBuilder.Attributes.Add("data-dialog-title", actionDialogFormParamters.DialogTitle);
            tBuilder.Attributes.Add("data-update-target-id", actionDialogFormParamters.UpdateTargetId);
            tBuilder.Attributes.Add("data-update-url", actionDialogFormParamters.UpdateUrl);
            tBuilder.Attributes.Add("data-update-url-index", actionDialogFormParamters.UpdateUrlFormIndexFieldName);
            tBuilder.Attributes.Add("data-onSuccessfunction", actionDialogFormParamters.OnSuccessClientFunction);
            tBuilder.Attributes.Add("data-action", actionDialogFormParamters.Action);

            tBuilder.AddCssClass("dialogLink");


            return new MvcHtmlString(tBuilder.ToString());
        }


        //public static MvcHtmlString ClientButton(this HtmlHelper htmlHelper, ButtonType buttonType, string buttonText, ButtonIcon buttonIcon, string onClickFunction = "", string link = "#")
        //{

        //    return ClientButton(htmlHelper, new ButtonParameters(buttonIcon, buttonType, buttonText), onClickFunction, link);
        //}

        //public static MvcHtmlString ClientButton(this HtmlHelper htmlHelper, ButtonType buttonType, ButtonIcon buttonIcon, string onClickFunction = "", string link = "#")
        //{
        //    return ClientButton(htmlHelper, new ButtonParameters(buttonIcon, buttonType), onClickFunction, link);
        //}

        public static MvcHtmlString ClientButton(this HtmlHelper htmlHelper, ButtonParametersBase buttonParameters,
            KnocktOutParameters knocktOutParameters)
        {
            return ClientButton(htmlHelper, buttonParameters,
                new ClientButtonFunctions().HtmlAttributesSet(new {data_bind = knocktOutParameters.DataBindString}));
        }


        public static MvcHtmlString ClientButton(this HtmlHelper htmlHelper, ButtonParametersBase buttonParameters,
            ClientButtonFunctions clientButtonFunctions)
        {
            return ClientButton(htmlHelper, buttonParameters, clientButtonFunctions.OnClickFunction,
                clientButtonFunctions.Link, clientButtonFunctions.CssClasses, clientButtonFunctions.HtmlAttributes);
        }

        public static MvcHtmlString ClientButton(this HtmlHelper htmlHelper, ButtonParametersBase buttonParameters, string onClickFunction = "", string link = "#",string cssClasses=null,object htmlAttributes=null)
        {

            if (!string.IsNullOrEmpty(onClickFunction))
            {
                if ((onClickFunction.LastIndexOf('(') > 0))
                {
                    if (onClickFunction[onClickFunction.LastIndexOf('(') + 1] == ')')
                        onClickFunction = onClickFunction.Substring(0, onClickFunction.LastIndexOf('(')) + "(caller)";
                }
            }
            var aLink = new TagBuilder("a");
            if (!string.IsNullOrEmpty(onClickFunction))
            {
                aLink.Attributes.Add("data-funcToCall",onClickFunction);
                aLink.Attributes.Add("onclick", "ClientButtonFunction(this)"); 
            }
            if(!string.IsNullOrEmpty(link))
                aLink.Attributes.Add("href",link);
            aLink.SetInnerText("REPLACE");
            var htmlStr = aLink.ToString();
            htmlStr = buttonParameters.ButtonConstructor(htmlStr).ToString();
            htmlStr = !string.IsNullOrEmpty(cssClasses)
                          ? HTMLHelper.AddClassToHtmlElement(htmlStr, cssClasses)
                          : htmlStr;
            if(htmlAttributes!=null)
                htmlStr = ParseTupledAnonymousObject(htmlAttributes)
                    .Aggregate(htmlStr, (current, prop) => HTMLHelper.AddAttributesToHtmlElement(current, string.Format("{0}='{1}'", prop.Key.Replace('_','-'), prop.Value)));
            return new MvcHtmlString(htmlStr);

        }

        private static IDictionary<string,string> ParseTupledAnonymousObject(object tuple)
        {
            var dict = new Dictionary<string, string>();
            var props = TypeDescriptor.GetProperties(tuple);
            foreach (PropertyDescriptor prop in props)
            {
                dict.Add(prop.Name,prop.GetValue(tuple) as string);
            }
            return dict;
        }
            
        [Obsolete]
        public static MvcHtmlString ClientCancelButton(this HtmlHelper htmlHelper, ButtonType buttonType = ButtonType.Image, string onClickFunction = "", string link = "#")
        {
            var but = ClientButton(htmlHelper,new ButtonParameters(ButtonIcon.Cancel), onClickFunction, link).ToString();
            but = HTMLHelper.AddClassToHtmlElement(but, "CloseIfInWindow");
            but = HTMLHelper.AddClassToHtmlElement(but, "form-cancel");
            return new MvcHtmlString(but);
        }

        public static MvcHtmlString FormCancelButton(this HtmlHelper htmlHelper)
        {
            return FormCancelButton(htmlHelper, new ButtonParameters(ButtonIcon.Cancel,ButtonHeight.Small));
        }

        public static MvcHtmlString FormCancelButton(this HtmlHelper htmlHelper, ButtonParametersBase buttonParameters)
        {
            var htmlStr = string.Format("<a>REPLACE</a>");
            htmlStr = buttonParameters.ButtonConstructor(htmlStr).ToString();
            htmlStr = HTMLHelper.AddClassToHtmlElement(htmlStr, "CloseIfInWindow");
            htmlStr = HTMLHelper.AddClassToHtmlElement(htmlStr, "form-cancel");
            return new MvcHtmlString(htmlStr);
        }

         public static MvcHtmlString FormCloseButton(this HtmlHelper htmlHelper, ButtonParametersBase buttonParameters,string functionToCall)
         {
             var htmlStr = string.Format("<a>REPLACE</a>");
             htmlStr = buttonParameters.ButtonConstructor(htmlStr).ToString();
             //htmlStr = HTMLHelper.AddClassToHtmlElement(htmlStr, "CloseIfInWindow");
             htmlStr = HTMLHelper.AddClassToHtmlElement(htmlStr, "form-close");
             htmlStr = HTMLHelper.AddAttributesToHtmlElement(htmlStr, string.Format("data-func='{0}'", functionToCall));
             return new MvcHtmlString(htmlStr);
         }
    }


    public enum ButtonIcon
    {
        Edit,
        Delete,
        List,
        Camera,
        Ok,
        Cancel,
        Refresh,
        Filter,
        FilterClear,
        Add,
        Return,
        Work,
        Document,
        Warning,
        Exchange,
        Search,
        ZoomIn,
        Check,
        Timer,
        Asterisk,
        Forward,
        Backward,
        Briefcase
    }

    public enum ButtonType
    {
        Image,
        ImageText
    }

    public enum ButtonRole
    {
        Default,
        Primary,
        Info,
        Success,
        Warning,
        Danger,
        Inverse
    }

    public enum ButtonIconColor
    {
        Black,
        White
    }

    public enum ButtonHeight
    {
        Normal,
        Large,
        Small,
        Mini
    }




    internal static class TelerikActionButtonHTML
    {
        private static Dictionary<ButtonType, string> _htmlButtonType = new Dictionary<ButtonType, string>()
                                                                                {
                                                                                    {ButtonType.Image,"t-button t-button-icon"},
                                                                                    {ButtonType.ImageText,"t-button t-button-icontext"}
                                                                    
                                                                                };

        private static Dictionary<ButtonIcon, string> _htmlButtonAction = new Dictionary<ButtonIcon, string>()
                                                                                {
                                                                                    {ButtonIcon.Add,"<span class='t-icon t-add'></span>INNERTEXT"},
                                                                                    {ButtonIcon.Delete,"<span class='t-icon t-delete'></span>INNERTEXT"},
                                                                                    {ButtonIcon.Ok,"<span class='t-icon t-update'></span>INNERTEXT"},
                                                                                    {ButtonIcon.Cancel,"<span class='t-icon t-cancel'></span>INNERTEXT"},
                                                                                    {ButtonIcon.Refresh,"<span class='t-icon t-refresh'></span>INNERTEXT"},
                                                                                    {ButtonIcon.Edit,"<span class='t-icon t-edit'></span>INNERTEXT"},
                                                                                    {ButtonIcon.Filter,"<span class='t-icon t-filter'></span>INNERTEXT"},
                                                                                    {ButtonIcon.FilterClear,"<span class='t-icon t-clear-filter'></span>INNERTEXT"},
                                                                                    {ButtonIcon.Search,"<span class='t-icon t-search'></span>INNERTEXT"},
                                                                                    {ButtonIcon.ZoomIn,"<i class='icon-zoom-in'></i> INNERTEXT"},
                                                                                    {ButtonIcon.List, PrepareHtmlForIcon("list_16x16.png","List","middle")},
                                                                                    {ButtonIcon.Return,PrepareHtmlForIcon("return_16x16.png","Return","top")},
                                                                                    {ButtonIcon.Work,PrepareHtmlForIcon("work_16x16.png","Work","top")},
                                                                                    {ButtonIcon.Camera,PrepareHtmlForIcon("camera_16x16.png","Camera","middle")},
                                                                                    {ButtonIcon.Warning,PrepareHtmlForIcon("warning_16x16.png","Warning","middle")},
                                                                                    {ButtonIcon.Document,PrepareHtmlForIcon("document_16x16.png","Document","middle")},
                                                                                    {ButtonIcon.Exchange,PrepareHtmlForIcon("exchange_16x16.png","Document","middle")}
                                                                    
                                                                                };

        private static string PrepareHtmlImgSrc(string imagePath, string alt, string align)
        {
            var htmlStr = string.Format(@"<img src='{0}' border='0' align='{1}' alt='{2}' /> INNERTEXT", VirtualPathUtility.ToAbsolute(imagePath), align, alt);
            return htmlStr;
        }

        private static string PrepareHtmlForIcon(string iconName, string alt, string align)
        {
            return PrepareHtmlImgSrc(string.Format(@"~/Content/site/icons/{0}", iconName), alt, align);
        }

        public static string GetClassForButtonType(ButtonType buttonType)
        {
            return _htmlButtonType[buttonType];
        }

        public static string GetInnerHtmlForbuttonIcon(ButtonIcon buttonIcon, string buttonText = "")
        {
            var html = _htmlButtonAction[buttonIcon].Replace("INNERTEXT", buttonText);
            return html;
        }

        public static MvcHtmlString ActionButtonConstructor(TelerikButtonParameters buttonParameters, string htmlStr)
        {
            htmlStr = HTMLHelper.AddClassToHtmlElement(htmlStr, GetClassForButtonType(buttonParameters.Type));
            htmlStr = (!buttonParameters.Enabled) ? HTMLHelper.AddStyleAttributeToHtmlElement(htmlStr, "visibility:hidden") : htmlStr;
            if (!string.IsNullOrEmpty(buttonParameters.ToolTip))
            {
                htmlStr = HTMLHelper.AddAttributesToHtmlElement(htmlStr, "rel='tooltip'");
                htmlStr = HTMLHelper.AddAttributesToHtmlElement(htmlStr, string.Format("title='{0}'", buttonParameters.ToolTip));
            }
            var innerHtml = GetInnerHtmlForbuttonIcon(buttonParameters.ButtonIcon, buttonParameters.Text);
            innerHtml = (!buttonParameters.Enabled) ? HTMLHelper.AddAttributesToHtmlElement(innerHtml, "disabled='disabled'") : innerHtml;
            htmlStr = (buttonParameters.Width != null) ? HTMLHelper.AddStyleAttributeToHtmlElement(htmlStr, string.Format("width:{0};text-align:left", buttonParameters.Width)) : htmlStr;
            htmlStr = htmlStr.Replace("REPLACE", innerHtml);
            return new MvcHtmlString(htmlStr);
        }
    }

    public static class BootstrapActionButtonHTML
    {

        private static Dictionary<ButtonRole, string> _htmlButtonRole = new Dictionary<ButtonRole, string>()
                                                                            {
                                                                                {ButtonRole.Default, ""},
                                                                                {ButtonRole.Primary,"btn-primary" },
                                                                                {ButtonRole.Info,"btn-info" },
                                                                                {ButtonRole.Warning,"btn-warning" },
                                                                                {ButtonRole.Danger,"btn-danger" },
                                                                                {ButtonRole.Success,"btn-success" },
                                                                                {ButtonRole.Inverse,"btn-inverse" },
                                                                            };

        private static Dictionary<ButtonIconColor, string> _htmlButtonIconColor = new Dictionary<ButtonIconColor, string>()
                                                                            {
                                                                                {ButtonIconColor.Black, ""},
                                                                                {ButtonIconColor.White,"icon-white" },
                                                                            };

        private static Dictionary<ButtonHeight, string> _htmlButtonHeight = new Dictionary<ButtonHeight, string>()
                                                                                {
                                                                                    {ButtonHeight.Normal, ""},
                                                                                    {ButtonHeight.Large, "btn-large"},
                                                                                    {ButtonHeight.Small, "btn-small"},
                                                                                    {ButtonHeight.Mini, "btn-mini"}
                                                                                };

        public static MvcHtmlString ActionButtonConstructor(ButtonParameters buttonParameters, string htmlStr)
        {
            htmlStr = HTMLHelper.AddClassToHtmlElement(htmlStr, string.Format("btn {0} {1} {2}", _htmlButtonRole[buttonParameters.ButtonRole]  
                                                                                                , _htmlButtonHeight[buttonParameters.ButtonHeight] 
                                                                                                , (!buttonParameters.Enabled?" .disabled":"")));
            if (!string.IsNullOrEmpty(buttonParameters.ToolTip))
            {
                htmlStr = HTMLHelper.AddAttributesToHtmlElement(htmlStr, "rel='tooltip'");
                htmlStr = HTMLHelper.AddAttributesToHtmlElement(htmlStr, string.Format("title='{0}'", buttonParameters.Text));
            }
            
            htmlStr = (!buttonParameters.Enabled) ? HTMLHelper.AddAttributesToHtmlElement(htmlStr, "disabled='disabled'") : htmlStr;
            htmlStr = (buttonParameters.ButtonId!=null) ? HTMLHelper.AddAttributesToHtmlElement(htmlStr, "id='"+buttonParameters.ButtonId+"'") : htmlStr;
            var innerHtml = string.Format("{0}{1}",IconConstructor.ConstructIcon(new IconParameters(buttonParameters.ButtonIcon,buttonParameters.ButtonIconColor))
                                                                  ,  buttonParameters.Type == ButtonType.ImageText ? " " + buttonParameters.Text:"");
            htmlStr = (buttonParameters.Width != null) ? HTMLHelper.AddStyleAttributeToHtmlElement(htmlStr, string.Format("width:{0};text-align:left", buttonParameters.Width)) : htmlStr;
            htmlStr = htmlStr.Replace("REPLACE", innerHtml);
            return new MvcHtmlString(htmlStr);
        }       
    }

}