using System;
using System.Web;
using C3.Web.Helpers;
using C3.Data;
using C3.Business;
using Phytel.Framework.SQL.Data;
using Atmosphere.Web;

namespace C3.Web.Controls
{

    public partial class Header : System.Web.UI.UserControl
    {
        #region Page Control Declarations
        /// <summary>
        /// updHeader control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.UpdatePanel updHeader;


        /// <summary>
        /// modal_changecontract_panel control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl modal_changecontract_panel;

        /// <summary>
        /// modal_changecontract control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl modal_changecontract;

        /// <summary>
        /// changeContractTitle control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl changeContractTitle;

        /// <summary>
        /// changeContractBodyText control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl changeContractBodyText;

        /// <summary>
        /// btnContinueChangeContract control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.Button btnContinueChangeContract;

        /// <summary>
        /// changeContractpageRedirect control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.HtmlControls.HtmlInputHidden changeContractpageRedirect;

        /// <summary>
        /// pnlContainer control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.Panel pnlContainer;

        /// <summary>
        /// pnlWelcome control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.Panel pnlWelcome;

        /// <summary>
        /// lblWelcome control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.Label lblWelcome;

        /// <summary>
        /// btnChangePassword control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.LinkButton btnChangePassword;

        /// <summary>
        /// btnSignOut control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.LinkButton btnSignOut;

        /// <summary>
        /// pnlSearchBox control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.Panel pnlSearchContainer;

        /// <summary>
        /// pnlSearchBox control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.Panel pnlSearchBox;

        /// <summary>
        /// btnPatientSearch control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.Button btnPatientSearch;

        /// <summary>
        /// txtPatientSearch control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.TextBox txtPatientSearch;

        #endregion

        #region Protected
        protected void Page_Load(object sender, EventArgs e)
        {
          
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (CurrentUser != null && Context.User.Identity.IsAuthenticated)
            {
                string displayName = CurrentUser.DisplayName ?? CurrentUser.UserName;

                if (displayName.Length > 20)
                    displayName = displayName.Substring(0, 20) + "...";

                lblWelcome.Text = displayName;
            }

            btnChangePassword.Visible = false; //hiding this as there is no need for it here...
            base.OnPreRender(e);
        }

        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
            UserHelper.RedirectUser(CurrentUser, UserHelper.GlobalSiteRoot + "ChangePassword.aspx", "ChangePassword", true);
        }
        
        protected void btnSignOut_Click(object sender, EventArgs e)
        {
            //Log Audit for Sign Out
            AuditData auditLog = AuditHelper.GetAuditLog();
            auditLog.Type = "SignOut";
            auditLog.SourcePage = System.IO.Path.GetFileName(Request.PhysicalPath);
            AuditService.Instance.LogEvent(auditLog);

            UserHelper.ClearUserSessionVariables(true);            
            System.Web.Security.FormsAuthentication.SignOut();
            UserHelper.RedirectUser(CurrentUser);
        }

        protected bool IsUserLoggedIn
        {
            get
            {
                return CurrentUser != null;
            }
        }

        protected C3User CurrentUser
        {
            get
            {
                C3User _user;
                _user = (C3User)Session["C3User"];
                if (_user != null)
                {
                    return _user;
                }
                else
                {
                    return null;
                }
            }
        }

        #endregion

    }
}