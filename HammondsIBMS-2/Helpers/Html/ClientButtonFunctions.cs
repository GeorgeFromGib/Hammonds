namespace HammondsIBMS_2.Helpers.Html
{
    public class ClientButtonFunctions
    {
        public string OnClickFunction = "";
        public string Link = "#";
        public string CssClasses = null;
        public object HtmlAttributes = null;

        public ClientButtonFunctions(string link="#")
        {
            Link = link;
        }

        public ClientButtonFunctions ClickFunction(string function)
        {
            OnClickFunction = function;
            return this;
        }

        public ClientButtonFunctions CssClassSet(string cssClasses)
        {
            CssClasses = cssClasses;
            return this;
        }

        public ClientButtonFunctions HtmlAttributesSet(object htmlAttributes)
        {
            HtmlAttributes = htmlAttributes;
            return this;
        }
    }

    public class KnocktOutParameters
    {
        public readonly string DataBindString;

        public KnocktOutParameters(string dataBindString)
        {
            DataBindString = dataBindString;
        }
    }
}