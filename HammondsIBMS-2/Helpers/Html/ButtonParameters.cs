using System;
using System.Web.Mvc;

namespace HammondsIBMS_2.Helpers.Html
{
    public abstract class ButtonParametersBase
    {  
        public ButtonIcon ButtonIcon { get; set; }
        public string Text { get; set; }
        public ButtonType Type { get; set; }
        public bool Enabled { get; set; }
        public string Width { get; set; }
        public string ToolTip { get; set; }
        public string ButtonId { get; set; }

        public abstract MvcHtmlString ButtonConstructor(string htmlStr);

        protected static string ProcessWidth(string width)
        {
            int dummy;
            if (!String.IsNullOrEmpty(width) && Int32.TryParse(width, out dummy)) width = width + "px";
            if (!String.IsNullOrEmpty(width) && width.ToUpper() == "MAX") width = "78%";
            return width;
        }

        public ButtonParametersBase Icon(ButtonIcon buttonIcon)
        {
            ButtonIcon = buttonIcon;
            return this;
        }

        public virtual ButtonParametersBase Id(string id)
        {
            ButtonId = id;
            return this;
        }

        public virtual ButtonParametersBase Tip(string tip)
        {
            ToolTip = tip;
            return this;
        }
    }

    public class TelerikButtonParameters:ButtonParametersBase
    {
        public TelerikButtonParameters(ButtonIcon icon,ButtonType type, string text,bool enabled,string width,string toolTip)
        {
            ButtonIcon = icon;
            Text = text;
            Type = type;
            Enabled = enabled;
            Width = ProcessWidth(width);
            ToolTip = toolTip;
        }

        public TelerikButtonParameters(ButtonIcon icon,string toolTip=null)
            : this(icon, ButtonType.Image, null, true, null,toolTip)
        {
        }

        public TelerikButtonParameters(ButtonIcon icon,ButtonType buttonType,string buttonText):this(icon,buttonType,buttonText,true,null,null)
        {
        }

        public override MvcHtmlString ButtonConstructor(string htmlStr)
        {
            return TelerikActionButtonHTML.ActionButtonConstructor(this, htmlStr);
        }


    }

    public class ButtonParameters : ButtonParametersBase
    {
      
        public ButtonRole ButtonRole { get; set; }
        public ButtonIconColor ButtonIconColor { get; set; }
        public ButtonHeight ButtonHeight { get; set; }
       

        public ButtonParameters(ButtonIcon icon,ButtonType type, string text,ButtonRole buttonRole,ButtonIconColor buttonIconColor,ButtonHeight buttonHeight,bool enabled,string width,string toolTip=null)
        {
            ButtonIcon = icon;
            Text = text;
            ButtonRole = buttonRole;
            ButtonIconColor = buttonIconColor;
            ButtonHeight = buttonHeight;
            Type = type;
            Enabled = enabled;
            Width = ProcessWidth(width);
            ToolTip = toolTip;
        }

        public new ButtonParameters Tip(string tip)
        {
            ToolTip = tip;
            return this;
        }

        public ButtonParameters Role(ButtonRole buttonRole)
        {
            ButtonRole = buttonRole;
            return this;
        }

        public ButtonParameters IconColor(ButtonIconColor buttonIconColor)
        {
            ButtonIconColor = buttonIconColor;
            return this;
        }

        public ButtonParameters Height(ButtonHeight buttonHeight)
        {
            ButtonHeight = buttonHeight;
            return this;
        }

        public ButtonParameters OfType(ButtonType buttonType, string text = null)
        {
            Type = buttonType;
            Text = text;
            return this;
        }

        public ButtonParameters Enable(bool enabled)
        {
            Enabled = enabled;
            return this;
        }

        public ButtonParameters SetWidth(string width)
        {
            Width = ProcessWidth(width);
            return this;
        }

        public ButtonParameters(ButtonIcon icon)
            : this(icon, ButtonIconColor.Black)
        { 
        }

        public ButtonParameters(ButtonIcon icon,ButtonIconColor iconColor)
            : this(icon,  iconColor, null)
        {
        }

        public ButtonParameters(ButtonIcon icon,ButtonIconColor iconColor,ButtonRole buttonRole,ButtonHeight buttonHeight)
            : this(icon, ButtonType.Image, null, buttonRole, iconColor,buttonHeight, true, null)
        {
        }

        public ButtonParameters(ButtonIcon icon, ButtonHeight buttonHeight)
            : this(icon, ButtonType.Image, null, ButtonRole.Default, ButtonIconColor.Black,buttonHeight, true, null)
        {
        }

        public ButtonParameters(ButtonIcon icon, ButtonHeight buttonHeight,bool enabled)
            : this(icon, ButtonType.Image, null, ButtonRole.Default, ButtonIconColor.Black, buttonHeight, enabled, null)
        {
        }

        public ButtonParameters(ButtonIcon icon,string buttonText)
            : this(icon, ButtonType.ImageText, buttonText, ButtonRole.Default, ButtonIconColor.Black,ButtonHeight.Normal, true, null)
        {
        }

        public ButtonParameters(ButtonIcon icon,ButtonIconColor iconColor,string buttonText)
            : this(icon, ButtonType.ImageText, buttonText, ButtonRole.Default, iconColor,ButtonHeight.Normal, true, null)
        {
        }

        public ButtonParameters(ButtonIcon icon, ButtonIconColor iconColor, ButtonRole buttonRole,string buttonText)
            : this(icon, ButtonType.ImageText, buttonText, buttonRole, iconColor, ButtonHeight.Normal, true, null)
        {
        }

        public ButtonParameters(ButtonIcon icon, ButtonType type, string buttonText,string toolTip=null)
            : this(icon, type, buttonText,ButtonRole.Default,ButtonIconColor.Black, ButtonHeight.Normal, true,null,toolTip)
        {
        }

        public ButtonParameters(ButtonIcon icon, ButtonType type, string buttonText,
                                bool enabled, string width)
            : this(icon, type, buttonText, ButtonRole.Default, ButtonIconColor.Black, ButtonHeight.Normal, enabled, width)
        {
        }

        public override MvcHtmlString ButtonConstructor(string htmlStr)
        {
            return BootstrapActionButtonHTML.ActionButtonConstructor(this, htmlStr);
        }
    }
}