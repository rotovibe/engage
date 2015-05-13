using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Web.UI.HtmlControls;
using System.Text;

namespace C3.Web
{
    public partial class C3 : System.Web.UI.MasterPage
    {
        #region global properties
        /// <summary>
        /// Head1 control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.HtmlControls.HtmlHead Head1;

        /// <summary>
        /// Link1 control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.HtmlControls.HtmlLink Link1;

        /// <summary>
        /// Link2 control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.HtmlControls.HtmlLink Link2;

        /// <summary>
        /// head control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.ContentPlaceHolder head;

        /// <summary>
        /// form1 control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.HtmlControls.HtmlForm form1;

        /// <summary>
        /// ToolkitScriptManager1 control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::AjaxControlToolkit.ToolkitScriptManager ToolkitScriptManager1;

        /// <summary>
        /// Header1 control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected Controls.Header headerControl;


        /// <summary>
        /// ModalDivs control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.ContentPlaceHolder ModalDivs;

        /// <summary>
        /// TopNav control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.ContentPlaceHolder TopNav;

        /// <summary>
        /// MasterUpdatePanel control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.UpdatePanel MasterUpdatePanel;

        /// <summary>
        /// formHeaderText control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.Label formHeaderText;

        /// <summary>
        /// hdnShowSupportMenu control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.HiddenField hdnShowSupportMenu;

        /// <summary>
        /// divInformation control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl divInformation;

        /// <summary>
        /// formInformationMessage control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.Label formInformationMessage;

        /// <summary>
        /// divError control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl divError;

        /// <summary>
        /// formErrorMessage control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.Label formErrorMessage;

        /// <summary>
        /// UpdateProgress1 control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.UpdateProgress UpdateProgress1;

        /// <summary>
        /// Content control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.ContentPlaceHolder Content;

        /// <summary>
        /// ScriptManagerProxy1 control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.ScriptManagerProxy ScriptManagerProxy1;

        /// <summary>
        /// ScriptFooter control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.ContentPlaceHolder ScriptFooter;

        /// <summary>
        /// Footer1 control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::C3.Web.Controls.Footer Footer1;
        #endregion


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            Literal literal = new Literal();
            StringBuilder sb = new StringBuilder();

            sb.Append("<link id=\"Link1\" rel=\"icon\" runat=\"server\" type=\"image/ico\" href=\"" +
                Atmosphere.UrlHelper.ResolveUrl("favicon.ico") +
                    "\" />" + Environment.NewLine);
            sb.Append("<link id=\"Link2\" rel=\"shortcut icon\" runat=\"server\" type=\"image/x-icon\" href=\"" +
                Atmosphere.UrlHelper.ResolveUrl("favicon.ico") +
                    "\" />" + Environment.NewLine);

            sb.Append("<link rel=\"stylesheet\" href=\"" +
                Atmosphere.UrlHelper.ResolveUrl("{asset_root}/Style/CQS/toolbox.css") +
                "\" type=\"text/css\" media=\"screen, projection\" />" + Environment.NewLine);

            sb.Append("<link rel=\"stylesheet\" href=\"" +
                Atmosphere.UrlHelper.ResolveUrl("{asset_root}/Style/CQS/formlayout.css") +
                "\" type=\"text/css\" media=\"screen, projection\" />" + Environment.NewLine);

            sb.Append("<link rel=\"stylesheet\" href=\"" +
                Atmosphere.UrlHelper.ResolveUrl("{asset_root}/Style/CSS/main.css") +
                "\" type=\"text/css\" media=\"screen, projection\" />" + Environment.NewLine);

            sb.Append("<!--[if IE 7]>" + Environment.NewLine);

            sb.Append("<link rel=\"stylesheet\" href=\"" +
                Atmosphere.UrlHelper.ResolveUrl("{asset_root}/Style/CSS/IE7/overrides.css") +
                "\" type=\"text/css\" media=\"screen, projection\" />" + Environment.NewLine);
            sb.Append("<![endif]-->" + Environment.NewLine);

            sb.Append("<!--[if IE 8]>" + Environment.NewLine);
            sb.Append("<link rel=\"stylesheet\" href=\"" +
                Atmosphere.UrlHelper.ResolveUrl("{asset_root}/Style/CSS/IE8/overrides.css") +
                "\" type=\"text/css\" media=\"screen, projection\" />" + Environment.NewLine);
            sb.Append("<![endif]-->" + Environment.NewLine);

            sb.Append("<!--[if !IE]>" + Environment.NewLine);
            sb.Append("<link rel=\"stylesheet\" href=\"" +
                Atmosphere.UrlHelper.ResolveUrl("{asset_root}/Style/CSS/OtherBrowser/overrides.css") +
                "\" type=\"text/css\" media=\"screen, projection\" />" + Environment.NewLine);
            sb.Append("<![endif]-->" + Environment.NewLine);

            sb.Append("<link rel=\"stylesheet\" href=\"" +
                Atmosphere.UrlHelper.ResolveUrl("{asset_root}/Style/CSS/TelerikReset.css") +
                "\" type=\"text/css\" media=\"screen, projection\" />" + Environment.NewLine);

            sb.Append("<link rel=\"stylesheet\" href=\"" +
                Atmosphere.UrlHelper.ResolveUrl("{asset_root}/Style/Atmosphere/Grid.Atmosphere.css") +
                "\" type=\"text/css\" media=\"screen, projection\" />" + Environment.NewLine);

            sb.Append("<link rel=\"stylesheet\" href=\"" +
                Atmosphere.UrlHelper.ResolveUrl("{asset_root}/Style/Atmosphere/Menu.Atmosphere.css") +
                "\" type=\"text/css\" media=\"screen, projection\" />" + Environment.NewLine);

            sb.Append("<link rel=\"stylesheet\" href=\"" +
                Atmosphere.UrlHelper.ResolveUrl("{asset_root}/Style/Atmosphere/Slider.Atmosphere.css") +
                "\" type=\"text/css\" media=\"screen, projection\" />" + Environment.NewLine);

            sb.Append("<link rel=\"stylesheet\" href=\"" +
                Atmosphere.UrlHelper.ResolveUrl("{asset_root}/Style/Atmosphere/ComboBox.Atmosphere.css") +
                "\" type=\"text/css\" media=\"screen, projection\" />" + Environment.NewLine);


            sb.Append("<link rel=\"stylesheet\" href=\"" +
                Atmosphere.UrlHelper.ResolveUrl("{asset_root}/Style/Atmosphere/Calendar.Atmosphere.css") +
                "\" type=\"text/css\" media=\"screen, projection\" />" + Environment.NewLine);

            sb.Append("<link rel=\"stylesheet\" href=\"" +
                Atmosphere.UrlHelper.ResolveUrl("{asset_root}/Style/Atmosphere/Input.Atmosphere.css") +
                "\" type=\"text/css\" media=\"screen, projection\" />" + Environment.NewLine);

            if (HttpContext.Current.Request.Browser.Browser.ToLower().Contains("safari"))
            {
                sb.Append("<link rel=\"stylesheet\" href=\"" + Atmosphere.UrlHelper.ResolveUrl("{asset_root}Style/CSS/Safari/overrides.css") + "\" type=\"text/css\" media=\"screen, projection\" />" + Environment.NewLine);
            }

            literal.Text = sb.ToString();
            Page.Header.Controls.AddAt(0, literal);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Header.DataBind();
        }


    }
}
