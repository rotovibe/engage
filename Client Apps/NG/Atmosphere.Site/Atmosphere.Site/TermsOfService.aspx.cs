using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using C3.Business;
using C3.Data;
using C3.Web.Helpers;

namespace C3.Web
{
    public partial class TermsOfService : BasePage
    {
        #region
        /// <summary>
        /// TOSPDF control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.HtmlControls.HtmlAnchor TOSPDF;
        
        /// <summary>
        /// divTOS control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.HtmlControls.HtmlGenericControl divTOS;
        
        /// <summary>
        /// btnNoAccept control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.Button btnNoAccept;
        
        /// <summary>
        /// btnAccept control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::System.Web.UI.WebControls.Button btnAccept;
        #endregion

        Data.TermsOfService tos = null;

        protected override void OnPreLoad(EventArgs e)
        {
            base.OnPreLoad(e);
            SetPageProperties();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                tos = new TOSService().GetLatestTOS();
                ViewState["TermsOfService"] = tos;
            }
            else
                tos = (Data.TermsOfService)ViewState["TermsOfService"];

            base.SetPageHeaderText("Terms of Service");
            base.DisplayInformationText = false;
            this.btnNoAccept.Focus();
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            try
            {
                divTOS.InnerHtml = tos.TermsOfServiceText;

                if (System.IO.File.Exists(Server.MapPath(string.Format("TermsOfService/TOS_{0}.pdf", tos.Version.ToString()))))
                {
                    TOSPDF.HRef = string.Format("TermsOfService/TOS_{0}.pdf", tos.Version.ToString());
                    TOSPDF.Visible = true;
                }
                else
                    TOSPDF.Visible = false;

                if (CurrentUser.LastTOSVersion > 0)
                {
                    CurrentUser.UserPageInfo.ErrorMessageCode = "INF_012";
                    SetPageErrorText(ApplicationMessageService.Instance.GetMessage("INF_012"));
                }
            }
            catch { }
        }

        #region private
        
        private void SetPageProperties()
        {
            //This is needed to make permissions work
            PhytelPageName = "TermsOfService";
        }

        #endregion

        protected void btnNoAccept_Click(object sender, EventArgs e)
        {
            CurrentUser.UserPageInfo.ErrorMessageCode = "ERR_013";

            UserHelper.ClearUserSessionVariables(true);
            System.Web.Security.FormsAuthentication.SignOut();
            Response.Redirect(GlobalSiteRoot + "Login.aspx");
        }

        protected void btnAccept_Click(object sender, EventArgs e)
        {
            CurrentUser.AcceptedLatestTOS = true;
            CurrentUser.Save();

            //Log audit of Terms of Service Acknowledgement            
            var auditLog = GetAuditLog();
            auditLog.Type = "TermsOfService";
            auditLog.TOSVersion = tos.Version;
            AuditService.Instance.LogEvent(auditLog);

            UserHelper.RedirectUser(CurrentUser);
        }
        
    }
}
