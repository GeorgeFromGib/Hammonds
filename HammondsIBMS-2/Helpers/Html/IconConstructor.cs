using System.Collections.Generic;
using System.Web.Mvc;

namespace HammondsIBMS_2.Helpers.Html
{
    internal static class IconConstructor
    {
        private static Dictionary<ButtonIcon, string> _iconCssClass = new Dictionary<ButtonIcon, string>()
                                                                          {
                                                                              {ButtonIcon.Add,"icon-plus"},
                                                                              {ButtonIcon.Delete,"icon-remove"},
                                                                              {ButtonIcon.Ok,"icon-ok"},
                                                                              {ButtonIcon.Cancel,"icon-remove"},
                                                                              {ButtonIcon.Refresh,"icon-refresh"},
                                                                              {ButtonIcon.Edit,"icon-pencil"},
                                                                              {ButtonIcon.Filter,"icon-filter"},
                                                                              {ButtonIcon.FilterClear,"icon-remove-circle"},
                                                                              {ButtonIcon.Search,"icon-search"},
                                                                              {ButtonIcon.ZoomIn,"icon-zoom-in" },
                                                                              {ButtonIcon.List, "icon-list"},
                                                                              {ButtonIcon.Return,"icon-backward"},
                                                                              {ButtonIcon.Work,"icon-cog"},
                                                                              {ButtonIcon.Camera,"icon-camera"},
                                                                              {ButtonIcon.Warning,"icon-warning-sign"},
                                                                              {ButtonIcon.Document,"icon-file"},
                                                                              {ButtonIcon.Check,"icon-check"},
                                                                              {ButtonIcon.Exchange,"icon-random"},
                                                                              {ButtonIcon.Timer,"icon-time"},
                                                                              {ButtonIcon.Asterisk,"icon-asterisk" },
                                                                              {ButtonIcon.Forward,"icon-forward" },
                                                                              {ButtonIcon.Backward,"icon-backward" },
                                                                              {ButtonIcon.Briefcase,"icon-briefcase"},
                                                                          };
        private static Dictionary<ButtonIconColor, string> _iconColorCssClass = new Dictionary<ButtonIconColor, string>()
                                                                               {
                                                                                   {ButtonIconColor.Black, ""},
                                                                                   {ButtonIconColor.White,"icon-white" },
                                                                               };

        public static MvcHtmlString ConstructIcon(IconParameters iconParameters)
        {
            var htmlStr = string.Format("<i class='{0} {1}'></i>", _iconCssClass[iconParameters.ButtonIcon], _iconColorCssClass[iconParameters.ButtonIconColor]);
            return new MvcHtmlString(htmlStr);
        }
    }
}