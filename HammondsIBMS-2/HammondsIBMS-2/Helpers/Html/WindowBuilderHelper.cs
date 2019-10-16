using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Web.Mvc.UI;
using Telerik.Web.Mvc.UI.Fluent;

namespace HammondsIBMS_2.Helpers.Html
{
    public static class WindowBuilderHelper
    {
        public static WindowBuilder CreateWindow(HtmlHelper helper, WindowParameters parameters)
        {
            var hlpr = helper.Telerik();
            var winName = parameters.Name.ToUpper().EndsWith("WINDOW") ? parameters.Name : parameters.Name + "Window";
            var wdw = hlpr.Window().Name(winName).Title(parameters.HeaderText);
            if (!String.IsNullOrEmpty(parameters.Controller))
                wdw.HtmlAttributes(new Dictionary<string, object>()
                                       {
                                           {"data-action",UrlHelper.GenerateContentUrl("~/"+parameters.Controller+"/"+parameters.Action,helper.ViewContext.HttpContext)  },
                                           {"data-values",HtmlHelperExtensions.SerializeToQueryString(parameters.RouteValues)}
                                       });
            //.LoadContentFrom(parameters.Action,parameters.Controller,parameters.RouteValues)
            var loadXpos = (parameters.WidthInPx / 2) - 16;
            var loadYpos = (parameters.HeightInPx / 2) - 16;
            wdw.Content("<div class='window-loading t-loading' style='position:relative;top:" + loadYpos + "px;left:" + loadXpos + "px;width:16px;height:16px;'> </div><div class='window-contents' style='overflow:auto;'></div>")
                .Width(parameters.WidthInPx).Height(parameters.HeightInPx)
                .Resizable().Modal(true).Visible(false).Draggable(true);
            return wdw;
        }

        public static WindowBuilder CreateWindow(HtmlHelper helper, ActionButtonWindowParameters actionButtonWindowParameters, ActionParameters actionParameters)
        {
            var hlpr = helper.Telerik();
            var winName = actionButtonWindowParameters.Name.ToUpper().EndsWith("WINDOW") ? actionButtonWindowParameters.Name : actionButtonWindowParameters.Name + "Window";
            var wdw = hlpr.Window().Name(winName).Title(actionButtonWindowParameters.HeaderText);
            var controller = String.IsNullOrEmpty(actionParameters.Controller)
                                 ? helper.ViewContext.RouteData.Values["controller"]
                                 : actionParameters.Controller;

            wdw.HtmlAttributes(new Dictionary<string, object>()
                                   {
                                       {"data-action",UrlHelper.GenerateContentUrl("~/"+controller+"/"+actionParameters.Action,helper.ViewContext.HttpContext)  },
                                       {"data-values",HtmlHelperExtensions.SerializeToQueryString(actionParameters.RouteValues)}
                                   });
            //.LoadContentFrom(parameters.Action,parameters.Controller,parameters.RouteValues)
            var loadXpos = (actionButtonWindowParameters.WidthInPx / 2) - 16;
            var loadYpos = (actionButtonWindowParameters.HeightInPx / 2) - 16;
            wdw.Content("<div class='window-loading t-loading' style='position:relative;top:" + loadYpos + "px;left:" + loadXpos + "px;width:16px;height:16px;'> </div><div class='window-contents' style='overflow:auto;'></div>")
                .Width(actionButtonWindowParameters.WidthInPx).Height(actionButtonWindowParameters.HeightInPx)
                .Resizable().Modal(true).Visible(false).Draggable(true);
            if (!String.IsNullOrEmpty(actionButtonWindowParameters.OnWindowCloseClientFunction))
                wdw.ClientEvents(ev => ev.OnClose(actionButtonWindowParameters.OnWindowCloseClientFunction));
            var wtb = actionButtonWindowParameters.TitleBarButtons;
            if ((wtb & ActionWindowTitleBarButtons.Close) == ActionWindowTitleBarButtons.Close)
                wdw.Buttons(buttons => buttons.Close());
            if ((wtb & ActionWindowTitleBarButtons.Maximise) == ActionWindowTitleBarButtons.Maximise)
                wdw.Buttons(buttons => buttons.Maximize());
            if ((wtb & ActionWindowTitleBarButtons.Refresh) == ActionWindowTitleBarButtons.Refresh)
                wdw.Buttons(buttons => buttons.Refresh());
            if ((wtb & ActionWindowTitleBarButtons.None) == ActionWindowTitleBarButtons.None)
                wdw.Buttons(buttons => buttons.Clear());
            return wdw;
        }
    }
}