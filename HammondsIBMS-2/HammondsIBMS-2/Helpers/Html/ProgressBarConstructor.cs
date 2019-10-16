using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;

namespace HammondsIBMS_2.Helpers.Html
{
    internal static class ProgressBarConstructor
    {
        private static Dictionary<ButtonRole, string> _htmlRole = new Dictionary<ButtonRole, string>()
                                                                      {
                                                                          {ButtonRole.Default, ""},
                                                                          {ButtonRole.Primary,"progress-primary" },
                                                                          {ButtonRole.Info,"progress-info" },
                                                                          {ButtonRole.Warning,"progress-warning" },
                                                                          {ButtonRole.Danger,"progress-danger" },
                                                                          {ButtonRole.Success,"progress-success" },
                                                                          {ButtonRole.Inverse,"progress-inverse" },
                                                                      };

        public static MvcHtmlString ConstructBar(string name,ButtonRole role,int value)
        {
            var htmlStr = new StringBuilder();
            htmlStr.Append(string.Format("<div class='progress {0}'>",_htmlRole[role]));
            htmlStr.Append(String.Format("<div  id='{0}' class='bar' style='width: {1}%;'>", name, value));
            htmlStr.Append("</div></div>");

            return new MvcHtmlString(htmlStr.ToString());
        }
    }
}