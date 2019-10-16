using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HammondsIBMS_2.Helpers.Html
{
    public enum BadgeType
    {
        Normal=0,
        Success=1,
        Warning=2,
        Important=3,
        Info=4,
        Inverse=5
    }

    public class IconParameters
    {
        public ButtonIcon ButtonIcon { get; set; }
        public ButtonIconColor ButtonIconColor { get; set; }


        public IconParameters(ButtonIcon buttonIcon,ButtonIconColor buttonIconColor=ButtonIconColor.Black)
        {
            ButtonIcon = buttonIcon;
            ButtonIconColor = buttonIconColor;
        }
    }

    public static class HtmlBadge
    {
        public static MvcHtmlString Badge(this HtmlHelper helper,BadgeType badgeType,string text)
        {
            return BadgeConstructor.ConstructBadge(badgeType, text);
        }

        public static MvcHtmlString Badge(this HtmlHelper helper,BadgeType badgeType,IconParameters icon)
        {
            return BadgeConstructor.ConstructBadge(badgeType, IconConstructor.ConstructIcon(icon).ToString());
        }

        public static MvcHtmlString Badge(this HtmlHelper helper,BadgeType badgeType,IconParameters icon,string text)
        {
            return BadgeConstructor.ConstructBadge(badgeType, IconConstructor.ConstructIcon(icon)+" "+text);
        }
    }

    internal static class BadgeConstructor
    {
        private static Dictionary<BadgeType, string> _badgeTypeCssClass = new Dictionary<BadgeType, string>
                                                                              {
                                                                                  {BadgeType.Normal, ""},
                                                                                  {BadgeType.Success,"badge-success" },
                                                                                  {BadgeType.Warning, "badge-warning"},
                                                                                  {BadgeType.Important,"badge-important" },
                                                                                  {BadgeType.Info,"badge-info" },
                                                                                  {BadgeType.Inverse,"badge-inverse" }
                                                                              };

        public static MvcHtmlString ConstructBadge(BadgeType badgeType, string text)
        {
            var htmlStr = String.Format(@"<span class='badge {0}'>{1}</span>",_badgeTypeCssClass[badgeType],text);
            return new MvcHtmlString(htmlStr);
        }
    }
}


